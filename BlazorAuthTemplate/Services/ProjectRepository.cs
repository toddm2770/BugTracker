using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using static BlazorAuthTemplate.Models.Enums;

namespace BlazorAuthTemplate.Services
{
	public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IServiceProvider svcProvider) : IProjectRepository
	{
		public async Task AddMemberToProjectAsync(int projectId, string userId, string managerId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
			if (manager == null) return;

			bool isAdmin = await userManager.IsInRoleAsync(manager, nameof(Roles.Admin));

			if (isAdmin == false)
			{
				ApplicationUser? projectManager = await GetProjectManagerAsync(projectId, manager.CompanyId);
				if (projectManager?.Id != managerId) return;
			} 

			ApplicationUser? userToAdd = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.CompanyId == manager.CompanyId);
			if (userToAdd == null) return;

			bool userIsProjectManager = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.ProjectManager));
			if (userIsProjectManager) return;

			bool userIsAdmin = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.Admin));
			if (userIsAdmin) return;

			Project? project = await context.Projects
											.Include(p => p.Members)
											.FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == manager.CompanyId);
			if (project == null) return;

			if (project.Members.Any(m => m.Id == userToAdd.Id) == false)
			{
				project.Members.Add(userToAdd);
				await context.SaveChangesAsync();
			}
		}

		public async Task<Project> AddProjectAsync(Project project, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			project.Created = DateTimeOffset.Now;
			
			context.Projects.Add(project);

			try
			{
				await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}


			return project;
		}

		public async Task ArchiveProjectAsync(int projectId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Project? project = await context.Projects.Include(p => p.Tickets)
											.FirstOrDefaultAsync(p => p.Id == projectId);

			if (project != null)
			{
				foreach(var ticket in project.Tickets)
				{
					ticket.IsArchivedByProject = true;
				}
				project.IsArchived = true;
				await context.SaveChangesAsync();
			}
		}

		public async Task AssignProjectManagerAsync(int projectId, string userId, string adminId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? admin = await context.Users.FindAsync(adminId);
			if (admin is null) return;

			bool isAdmin = admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.Admin));
			bool isPM = admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.ProjectManager));

			if (isAdmin == true || (isPM == true && userId == adminId))
			{
				ApplicationUser? projectManager = await context.Users.FindAsync(userId);

				if (projectManager is not null && projectManager.CompanyId == admin!.CompanyId && await userManager.IsInRoleAsync(projectManager, nameof(Roles.ProjectManager)))
				{
					await RemoveProjectManagerAsync(projectId, adminId);

					Project? project = await context.Projects
													.Include(p => p.Members)
													.FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == admin.CompanyId);

					if (project != null)
					{
						project.Members!.Add(projectManager);
						await context.SaveChangesAsync();
					}
				}
			}
		}

		public async Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			List<Project> projects = await context.Projects
												  .Where(p => p.CompanyId == companyId && p.IsArchived == false)
												  .Include(p => p.Tickets)
												  .Include(p => p.Members)
												  .Include(p => p.Company)
												  .ToListAsync();
			return projects;
		}

		public async Task<IEnumerable<Project>> GetArchivedProjects(int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			List<Project> projects = await context.Projects
												  .Where(p => p.CompanyId == companyId && p.IsArchived == true)
												  .Include(p => p.Tickets)	
												  .Include(p => p.Members)
												  .Include(p => p.Company)
												  .ToListAsync();
			return projects;
		}

		public async Task<Project?> GetProjectByCompanyId(int projectId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Project? project = await context.Projects
											.Include(p => p.Tickets)
											.Include(p => p.Members)
											.Include(p => p.Company)
											.FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);
			return project;
		}

		public async Task<ApplicationUser?> GetProjectManagerAsync(int projectId, int companyId)
		{
			IEnumerable<ApplicationUser> projectMembers = await GetProjectMembersAsync(projectId, companyId);

			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			foreach (ApplicationUser member in projectMembers)
			{
				bool isProjectManager = await userManager.IsInRoleAsync(member, nameof(Roles.ProjectManager));

				if(isProjectManager == true)
				{
					return member;
				}
			}

			return null;
		}

		public async Task<IEnumerable<ApplicationUser>> GetProjectMembersAsync(int projectId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Project? project = await context.Projects
											.Include(p => p.Members)
											.FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);
			return project?.Members ?? [];
		}

		public async Task RemoveMemberFromProjectAsync(int projectId, string userId, string managerId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
			if (manager is null) return;

			Project? project = await context.Projects
											.Include(p => p.Members)
											.FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == manager.CompanyId);
			if(project is null) return;

			ApplicationUser? memberToRemove = project.Members.FirstOrDefault(m => m.Id == userId);
			if(memberToRemove is null) return;

			project.Members.Remove(memberToRemove);
			await context.SaveChangesAsync();
		}

		public async Task RemoveProjectManagerAsync(int projectId, string adminId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? admin = await userManager.FindByIdAsync(adminId);
			if (admin == null) return;

			ApplicationUser? projectManager = await GetProjectManagerAsync(projectId, admin.CompanyId);

			if (projectManager is null) return;

			if (await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
			{
				await RemoveMemberFromProjectAsync(projectId, projectManager.Id, adminId);
			}
		}

		public async Task RestoreProjectAsync(int projectId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Project? project = await context.Projects.Include(p => p.Tickets)
													 .FirstOrDefaultAsync(p => p.Id == projectId);

			foreach(var ticket in project.Tickets)
			{
				ticket.IsArchivedByProject = false;
			}

			if (project != null)
			{
				project.IsArchived = false;
				await context.SaveChangesAsync();
			}
		}

		public async Task UpdateProjectAsync(Project project, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			context.Update(project);
			await context.SaveChangesAsync();
		}
	}
}

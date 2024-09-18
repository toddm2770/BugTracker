using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace BlazorAuthTemplate.Services
{
	public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IProjectRepository
	{
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

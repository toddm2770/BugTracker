using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuthTemplate.Services
{
	public class ProjectRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IProjectRepository
	{
		public Task<Project> AddProjectAsync(Project project, int companyId)
		{
			throw new NotImplementedException();
		}

		public Task ArchiveProjectAsync(int projectId, int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			List<Project> projects = await context.Projects
												  .Where(c => c.CompanyId == companyId)
												  .Include(p => p.Tickets)
												  .Include(p => p.Members)
												  .ToListAsync();
			return projects;
		}

		public Task<IEnumerable<Project>> GetArchivedProjects(int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task<Project?> GetProjectByCompanyId(int projectId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Project? project = await context.Projects
											.Include(p => p.Tickets)
											.Include(p => p.Members)
											.FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == companyId);
			return project;
		}

		public Task RestoreProjectAsync(int projectId, int companyId)
		{
			throw new NotImplementedException();
		}

		public Task UpdateProjectAsync(Project projectId, int companyId)
		{
			throw new NotImplementedException();
		}
	}
}

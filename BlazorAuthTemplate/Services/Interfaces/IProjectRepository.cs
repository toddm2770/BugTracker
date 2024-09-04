using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Models;

namespace BlazorAuthTemplate.Services.Interfaces
{
	public interface IProjectRepository
	{
		Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId);

		Task<IEnumerable<Project>> GetArchivedProjects(int companyId);

		Task<Project?> GetProjectByCompanyId(int projectId, int companyId);

		Task<Project> AddProjectAsync(Project project, int companyId);

		Task UpdateProjectAsync(Project projectId, int companyId);

		Task ArchiveProjectAsync(int projectId, int companyId);

		Task RestoreProjectAsync(int projectId, int companyId);
	}
}

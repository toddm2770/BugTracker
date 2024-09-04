using BlazorAuthTemplate.Client.Models;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
	public interface IProjectService
	{
		Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId);

		Task<IEnumerable<ProjectDTO>> GetArchivedProjects(int companyId);

		Task<ProjectDTO?> GetProjectByCompanyId(int projectId, int companyId);

		Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId);

		Task UpdateProjectAsync(ProjectDTO project, int companyId);

		Task ArchiveProjectAsync(int projectId, int companyId);

		Task RestoreProjectAsync(int projectId, int companyId);
	}
}

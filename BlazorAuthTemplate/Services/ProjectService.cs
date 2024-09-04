using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;

namespace BlazorAuthTemplate.Services
{
	public class ProjectService : IProjectService
	{

		private readonly IProjectRepository _repository;

		public ProjectService(IProjectRepository repository) 
		{
			_repository = repository;
		}

		public Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId)
		{
			throw new NotImplementedException();
		}

		public Task ArchiveProjectAsync(int projectId, int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
		{
			IEnumerable<Project> projects = await _repository.GetAllProjectsAsync(companyId);

			return projects.Select(p => p.ToDTO());
		}

		public Task<IEnumerable<ProjectDTO>> GetArchivedProjects(int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task<ProjectDTO?> GetProjectByCompanyId(int projectId, int companyId)
		{
			Project? project = await _repository.GetProjectByCompanyId(projectId, companyId);

			return project?.ToDTO();
		}

		public Task RestoreProjectAsync(int projectId, int companyId)
		{
			throw new NotImplementedException();
		}



		public Task UpdateProjectAsync(ProjectDTO project, int companyId)
		{
			throw new NotImplementedException();
		}
	}
}

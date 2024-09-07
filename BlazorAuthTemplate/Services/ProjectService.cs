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

		public async Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId)
		{
			Project newProject = new()
			{
				Name = project.Name,
				Description = project.Description,
				CompanyId = companyId,
				Priority = project.Priority,
				Created = DateTimeOffset.Now,
				StartDate = project.StartDate,
				EndDate = project.EndDate
			};

			newProject = await _repository.AddProjectAsync(newProject, companyId);

			return newProject.ToDTO();

		}

		public async Task ArchiveProjectAsync(int projectId, int companyId)
		{
			await _repository.ArchiveProjectAsync(projectId, companyId);
		}

		public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
		{
			IEnumerable<Project> projects = await _repository.GetAllProjectsAsync(companyId);

			return projects.Select(p => p.ToDTO());
		}

		public async Task<IEnumerable<ProjectDTO>> GetArchivedProjects(int companyId)
		{
			IEnumerable<Project> projects = await _repository.GetArchivedProjects(companyId);

			return projects.Select(p => p.ToDTO());
		}

		public async Task<ProjectDTO?> GetProjectByCompanyId(int projectId, int companyId)
		{
			Project? project = await _repository.GetProjectByCompanyId(projectId, companyId);

			return project?.ToDTO();
		}

		public async Task RestoreProjectAsync(int projectId, int companyId)
		{
			await _repository.RestoreProjectAsync(projectId, companyId);
		}



		public async Task UpdateProjectAsync(ProjectDTO project, int companyId)
		{
				Project? originalProject = await _repository.GetProjectByCompanyId(project.Id, companyId);

			if (originalProject == null) { return; }

			originalProject.Name = project.Name;
			originalProject.Description = project.Description;
			originalProject.StartDate = project.StartDate;
			originalProject.EndDate = project.EndDate;
			originalProject.Priority = project.Priority;

			await _repository.UpdateProjectAsync(originalProject, companyId);

		}
	}
}

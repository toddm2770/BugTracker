using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using static BlazorAuthTemplate.Models.Enums;

namespace BlazorAuthTemplate.Services
{
	public class ProjectService : IProjectService
	{

		private readonly IProjectRepository _repository;

		private readonly ICompanyRepository _companyRepository;

		public ProjectService(IProjectRepository repository, ICompanyRepository companyRepository)
		{
			_repository = repository;
			_companyRepository = companyRepository;
		}

		public async Task AddMemberToProjectAsync(int projectId, string userId, string managerId)
		{
			await _repository.AddMemberToProjectAsync(projectId, userId, managerId);
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

		public async Task AssignProjectManagerAsync(int projectId, string userId, string adminId)
		{
			await _repository.AssignProjectManagerAsync(projectId, userId, adminId);
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

		public async Task<UserDTO?> GetProjectManagerAsync(int projectId, int companyId)
		{
			ApplicationUser? projectManager = await _repository.GetProjectManagerAsync(projectId, companyId);
			if (projectManager is null) return null;

			UserDTO userDTO = projectManager.ToDTO();
			userDTO.Role = nameof(Roles.ProjectManager);

			return userDTO;
		}

		public async Task<IEnumerable<UserDTO>> GetProjectMembersAsync(int projectId, int companyId)
		{
			IEnumerable<ApplicationUser> members = await _repository.GetProjectMembersAsync(projectId, companyId);

			List<UserDTO> result = [];

			foreach(ApplicationUser member in members)
			{
				UserDTO userDTO = member.ToDTO();
				userDTO.Role = await _companyRepository.GetUserRoleAsync(member.Id, companyId);
				result.Add(userDTO);
			}

			return result;
		}

		public async Task RemoveMemberFromProjectAsync(int projectId, string userId, string managerId)
		{
			await _repository.RemoveMemberFromProjectAsync(projectId, userId, managerId);
		}

		public async Task RemoveProjectManagerAsync(int projectId, string adminId)
		{
			await _repository.RemoveProjectManagerAsync(projectId, adminId);
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

using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace BlazorAuthTemplate.Client.Services
{
	public class ProjectService : IProjectService
	{

		private readonly HttpClient _httpClient;

		public ProjectService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ProjectDTO> AddProjectAsync(ProjectDTO project, int companyId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/projects", project);
				response.EnsureSuccessStatusCode();

				ProjectDTO? createdProject = await response.Content.ReadFromJsonAsync<ProjectDTO>();
				return createdProject!;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task ArchiveProjectAsync(int projectId, int companyId)
		{
			HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/projects/{projectId}", projectId);
			response.EnsureSuccessStatusCode();
		}

		public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
		{
			var projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>($"api/projects") ?? [];
			return projects;
		}

		public async Task<IEnumerable<ProjectDTO>> GetArchivedProjects(int companyId)
		{
			var projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>($"api/projects") ?? [];
			return projects.Where(p => p.IsArchived == true);
		}

		public async Task<ProjectDTO?> GetProjectByCompanyId(int projectId, int companyId)
		{
			return await _httpClient.GetFromJsonAsync<ProjectDTO>($"api/projects/{projectId}, {companyId}");
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

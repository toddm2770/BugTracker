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
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/projects/{projectId}/Archive", projectId);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

		}

		public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync(int companyId)
		{
			try
			{
				var projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>($"api/projects") ?? [];
				return projects;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

		}

		public async Task<IEnumerable<ProjectDTO>> GetArchivedProjects(int companyId)
		{
			try
			{
				var projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDTO>>($"api/projects/GetArchivedProjects") ?? [];
				return projects;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

		}

		public async Task<ProjectDTO?> GetProjectByCompanyId(int projectId, int companyId)
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<ProjectDTO>($"api/projects/{projectId}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

		}

		public async Task RestoreProjectAsync(int projectId, int companyId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/projects/{projectId}/Restore", projectId);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

		}

		public async Task UpdateProjectAsync(ProjectDTO project, int companyId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/projects", project);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

		}
	}
}

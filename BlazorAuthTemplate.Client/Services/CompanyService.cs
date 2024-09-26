using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace BlazorAuthTemplate.Client.Services
{
	public class CompanyService : ICompanyService
	{
		private readonly HttpClient _httpClient;

		public CompanyService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task AddUserToRoleAsync(string userId, string roleName, string adminId)
		{
			throw new NotImplementedException();
		}

		public async Task<CompanyDTO?> GetCompanyByIdAsync(int id)
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<CompanyDTO>($"api/company/{id}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
		}

		public async Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId)
		{
			try
			{
				IEnumerable<UserDTO> members = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>($"api/company/members");
				return members ?? Enumerable.Empty<UserDTO>();
				
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task<string> GetUserRoleAsync(string userId, int companyId)
		{
			try
			{
				string userRoll = await _httpClient.GetStringAsync($"api/company/getRole?userId={userId}&companyId={companyId}");
				return userRoll;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
		{
			try
			{
				var users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>($"api/company/{roleName}");
				return users ?? Enumerable.Empty<UserDTO>();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public Task UpdateCompanyAsync(CompanyDTO company, string adminId)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateUserRoleAsync(UserDTO user, string adminId)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync($"api/company/user-role?adminId={adminId}", user);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}

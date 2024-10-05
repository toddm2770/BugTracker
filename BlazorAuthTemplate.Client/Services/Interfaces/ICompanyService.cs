using BlazorAuthTemplate.Client.Models;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
	public interface ICompanyService
	{
		Task<CompanyDTO?> GetCompanyByIdAsync(int id);

		Task<string> GetUserRoleAsync(string userId, int companyId);

		Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId);

		Task AddUserToRoleAsync(string userId, string roleName, string adminId);

		Task UpdateCompanyAsync(CompanyDTO company, string adminId);

		Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId);

		Task UpdateUserRoleAsync(UserDTO user, string adminId);

		Task<CompanyDTO> CreateCompanyAsync(CompanyDTO company);

		Task CreateAdmin(string userId, int companyId);
	}
}

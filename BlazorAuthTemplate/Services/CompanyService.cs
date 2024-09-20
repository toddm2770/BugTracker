using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;

namespace BlazorAuthTemplate.Services
{
	public class CompanyService : ICompanyService
	{
		private readonly ICompanyRepository _repository;

		public CompanyService(ICompanyRepository repository)
		{
			_repository = repository;
		}

		public Task AddUserToRoleAsync(string userId, string roleName, string adminId)
		{
			throw new NotImplementedException();
		}

		public async Task<CompanyDTO?> GetCompanyByIdAsync(int id)
		{
			Company? company = await _repository.GetCompanyByIdAsync(id);

			return company.ToDTO();
		}

		public async Task<string> GetUserRoleAsync(string userId, int companyId)
		{
			string roles = await _repository.GetUserRoleAsync(userId, companyId);

			return roles;
		}

		public async Task<IEnumerable<UserDTO>> GetUsersInRoleAsync(string roleName, int companyId)
		{
			IEnumerable<ApplicationUser> users = await _repository.GetUsersInRoleAsync(roleName, companyId);

			IEnumerable<UserDTO> userDTOs = users.Select(u => u.ToDTO());

			foreach(UserDTO user in userDTOs)
			{
				user.Role = roleName;
			}

			return userDTOs;
		}

		public Task UpdateCompanyAsync(CompanyDTO company, string adminId)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<UserDTO>> GetCompanyMembersAsync(int companyId)
		{
			Company? company = await _repository.GetCompanyByIdAsync(companyId);
			if (company is null) return [];

			List<UserDTO> members = [];

			foreach(ApplicationUser user in company.Members)
			{
				UserDTO member = user.ToDTO();
				member.Role = await _repository.GetUserRoleAsync(user.Id, companyId);
				members.Add(member);
			}

			return members;
		}

		public async Task UpdateUserRoleAsync(UserDTO user, string adminId)
		{
			if (string.IsNullOrEmpty(user.Role)) { return; }

			await _repository.AddUserToRoleAsync(user.Id, user.Role, adminId);
		}
	}
}

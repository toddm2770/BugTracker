using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using static BlazorAuthTemplate.Models.Enums;

namespace BlazorAuthTemplate.Services
{
	public class CompanyRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IServiceProvider svcProvider) : ICompanyRepository
	{
		public async Task AddUserToRoleAsync(string userId, string roleName, string adminId)
		{
			if (userId == adminId) { return; }

			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? admin = await userManager.FindByIdAsync(adminId);

			if (admin is not null && await userManager.IsInRoleAsync(admin, nameof(Roles.Admin)))
			{
				ApplicationUser? user = await userManager.FindByIdAsync(userId);

				if (user is not null && user.CompanyId == admin.CompanyId)
				{
					IList<string> currentRoles = await userManager.GetRolesAsync(user);
					string? currentRole = currentRoles.FirstOrDefault(r => r != nameof(Roles.DemoUser));

					if (string.Equals(currentRole, roleName, StringComparison.OrdinalIgnoreCase)) { return; }

					if (!string.IsNullOrEmpty(currentRole))
					{
						await userManager.RemoveFromRoleAsync(user, currentRole);
					}

					await userManager.AddToRoleAsync(user, roleName);
				}
			}
		}

		public async Task<Company> CreateCompanyAsync(Company company)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			context.Companies.Add(company);

			try
			{
				await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

			return company;
		}

		public async Task CreateAdmin(string userId, int companyId)
		{
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? user = await userManager.FindByIdAsync(userId);

			if (user != null)
			{
				await userManager.AddToRoleAsync(user, "Admin");
			}
		}

		public async Task<Company?> GetCompanyByIdAsync(int id)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Company? company = await context.Companies
											.Include(c => c.Projects)
											.Include(c => c.Invites)
											.Include(c => c.Members)
											.FirstOrDefaultAsync(c => c.Id == id);
			return company;
		}

		public async Task<string> GetUserRoleAsync(string userId, int companyId)
		{
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? user = await userManager.FindByIdAsync(userId);

			string role = "unknown";

			if(user?.CompanyId == companyId)
			{
				IList<string> roles = await userManager.GetRolesAsync(user);

				role = roles.FirstOrDefault(r => r != nameof(Roles.DemoUser), role);
			}

			return role;
		}

		public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName, int companyId)
		{
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			IList<ApplicationUser> users = await userManager.GetUsersInRoleAsync(roleName);
			IEnumerable<ApplicationUser> companyUsers = users.Where(u => u.CompanyId == companyId);

			return companyUsers;
		}

		public Task UpdateCompanyAsync(Company company, string adminId)
		{
			throw new NotImplementedException();
		}
	}
}

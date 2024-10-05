

using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuthTemplate.Controllers
{
	[Route("api/[Controller]")]
	[Authorize]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		private readonly ICompanyService _companyService;
		private readonly UserManager<ApplicationUser> _userManager;

		private int CompanyId => int.Parse(User.FindFirst("CompanyId")!.Value);

		private string UserId => _userManager.GetUserId(User)!;

		public CompanyController(ICompanyService companyService, UserManager<ApplicationUser> userManager)
		{
			_companyService = companyService;
			_userManager = userManager;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<CompanyDTO>> GetCompany([FromRoute] int id)
		{
			CompanyDTO? company = await _companyService.GetCompanyByIdAsync(CompanyId);

			if (company == null)
			{
				return NotFound();
			}

			return Ok(company);
		}

		[HttpPut("user-role")]
		public async Task<ActionResult> AddUserRole([FromBody] UserDTO user, [FromQuery] string adminId)
		{
			try
			{
				await _companyService.UpdateUserRoleAsync(user, adminId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		[HttpGet("getRole")]
		public async Task<ActionResult<string>> GetUserRole([FromQuery] string userId, [FromQuery] int companyId)
		{
			try
			{
				string userRoll = await _companyService.GetUserRoleAsync(userId, companyId);
				return userRoll;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		[HttpGet("{roleName}")]
		public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsersInRole([FromRoute] string roleName)
		{
			try
			{
				IEnumerable<UserDTO> usersInRoll = await _companyService.GetUsersInRoleAsync(roleName, CompanyId);
				return Ok(usersInRoll);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		[HttpGet("members")]
		public async Task<ActionResult<IEnumerable<UserDTO>>> GetMembers()
		{
			try
			{
				IEnumerable<UserDTO> users = await _companyService.GetCompanyMembersAsync(CompanyId);
				return Ok(users);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		[HttpPost("create")]
		public async Task<ActionResult<CompanyDTO>> CreateCompany([FromBody] CompanyDTO company)
		{
			try
			{
				CompanyDTO createdCompany = await _companyService.CreateCompanyAsync(company);
				return createdCompany;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		[HttpPut("admin/{userId}")]
		public async Task<ActionResult> CreateAdmin([FromRoute] string userId, [FromBody] int companyId)
		{
			try
			{
				await _companyService.CreateAdmin(userId, companyId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		[HttpPut("update/{company}")]
		public async Task<ActionResult> UpdateProject([FromRoute] CompanyDTO company, [FromBody] string adminId)
		{
			try
			{
				await _companyService.UpdateCompanyAsync(company, adminId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}

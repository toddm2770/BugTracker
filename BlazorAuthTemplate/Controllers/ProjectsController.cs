using BlazorAuthTemplate.Client;
using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Components.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuthTemplate.Controllers
{
	[Route("api/[Controller]")]
	[Authorize]
	[ApiController]
	public class ProjectsController : ControllerBase
	{
		private readonly IProjectService _projectService;

		public ProjectsController(IProjectService projectService)
		{
			_projectService = projectService;
		}

		private int _companyId => int.Parse(User.FindFirst("CompanyId")!.Value);

		[HttpPost]
		public async Task<ActionResult<ProjectDTO>> CreateProject([FromBody] ProjectDTO project)
		{
			try
			{
				ProjectDTO createdProject = await _projectService.AddProjectAsync(project, _companyId);
				return createdProject;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjects()
		{
			try
			{
				IEnumerable<ProjectDTO> projects = [];

				projects = await _projectService.GetAllProjectsAsync(_companyId);
				
				return Ok(projects);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpGet("GetArchivedProjects")]
		public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetArchivedProjects()
		{
			try
			{
				IEnumerable<ProjectDTO> projects = [];

				projects = await _projectService.GetArchivedProjects(_companyId);

				return Ok(projects);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpGet("{projectId:int}")]
		public async Task<ActionResult<ProjectDTO>> GetProjectById([FromRoute] int projectId)
		{
			try
			{
				ProjectDTO? project = await _projectService.GetProjectByCompanyId(projectId, _companyId);

				return Ok(project);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut("{projectId}/Archive")]
		public async Task<ActionResult> ArchiveProject([FromRoute] int projectId)
		{
			try
			{
				await _projectService.ArchiveProjectAsync(projectId, _companyId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut("{projectId}/Restore")]
		public async Task<ActionResult> RestoreProject([FromRoute] int projectId)
		{
			try
			{
				await _projectService.RestoreProjectAsync(projectId, _companyId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateProject([FromBody] ProjectDTO project)
		{
			try
			{
				await _projectService.UpdateProjectAsync(project, _companyId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}
	}
}

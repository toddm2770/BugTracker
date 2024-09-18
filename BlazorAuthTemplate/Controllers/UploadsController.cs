using BlazorAuthTemplate.Client.Helpers;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class UploadsController(ApplicationDbContext context) : ControllerBase
{
	private int? _companyId => User.FindFirst("CompanyId") != null ? int.Parse(User.FindFirst("CompanyId")!.Value) : null;

	[HttpGet("{id:guid}")]
	[OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60 * 1)]
	public async Task<IActionResult> GetImage(Guid id)
	{
		FileUpload? file = await context.Uploads.FirstOrDefaultAsync(i => i.Id == id);

		if (file is null) return NotFound();

		var ticketAttachment = await context.TicketAttachments
			.Include(ta => ta.Ticket)
				.ThenInclude(t => t!.Project)
			.FirstOrDefaultAsync(ta => ta.FileUploadId == id);

		if (ticketAttachment is null)
		{
			// this must be a regular image, no need to check company ID
			return File(file.Data!, file.Type!);
		}
		else
		{
			// this must be an attachment to a ticket, so make sure the user
			// is part of the company this ticket belongs to
			if (_companyId is null || ticketAttachment.Ticket?.Project?.CompanyId != _companyId)
			{
				return Unauthorized();
			}
			else
			{
				return File(file.Data!, file.Type!, ticketAttachment.FileName);
			}
		}

	}

	[HttpGet("author")] // api/uploads/author
	[OutputCache(Duration = 60 * 60)]
	public async Task<IActionResult> GetAuthorImage([FromServices] IConfiguration config)
	{
		string? authorEmail = config["AdminEmail"] ?? Environment.GetEnvironmentVariable("AdminEmail");

		ApplicationUser? author = await context.Users
											   .Include(u => u.Image)
											   .FirstOrDefaultAsync(u => u.Email == authorEmail);

		if (author?.Image is not null)
		{
			return File(author.Image.Data!, author.Image.Extension!);
		}
		else
		{
			string extension = ImageHelper.DefaultProfilePicture.Split('.')[^1];
			if (extension == "svg") extension = "svg+xml";

			return File(ImageHelper.DefaultProfilePicture, $"image/{extension}");
		}
	}

}

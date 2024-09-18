using BlazorAuthTemplate.Client.Helpers;
using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Components.Account;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Helpers;
using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace BlazorAuthTemplate.Controllers
{
	[Route("api/[Controller]")]
	[Authorize]
	[ApiController]
	public class TicketsController : ControllerBase
	{
		private readonly ITicketService _ticketsService;

		private readonly UserManager<ApplicationUser> _userManager;

		public TicketsController(ITicketService ticketsService, UserManager<ApplicationUser> userManager)
		{
			_ticketsService = ticketsService;
			_userManager = userManager;
		}

		private int _companyId => int.Parse(User.FindFirst("CompanyId")!.Value);

		private string _userId => User.GetUserId()!;

		[HttpPost]
		public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] TicketDTO ticket)
		{
			try
			{
				TicketDTO createdTicket = await _ticketsService.AddTicketAsync(ticket, _companyId);

				return createdTicket;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
		{
			try
			{
				IEnumerable<TicketDTO> tickets = [];

				tickets = await _ticketsService.GetAllTicketsAsync(_companyId);

				return Ok(tickets);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut("{ticketId}/Archive")]
		public async Task<ActionResult> ArchiveTicket([FromRoute] int ticketId)
		{
			try
			{
				await _ticketsService.ArchiveTicketAsync(ticketId, _companyId);

				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut("{ticketId}/Restore")]
		public async Task<ActionResult> RestoreTicket([FromRoute] int ticketId)
		{
			try
			{
				await _ticketsService.RestoreTicketAsync(ticketId, _companyId);

				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpGet("{ticketId:int}")]
		public async Task<ActionResult<TicketDTO>> GetTicketById([FromRoute] int ticketId)
		{
			try
			{
				TicketDTO? ticket = await _ticketsService.GetTicketByIdAsync(ticketId, _companyId);

				return Ok(ticket);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut]
		public async Task<ActionResult> UpdateTicket([FromBody] TicketDTO ticket)
		{
			try
			{
				await _ticketsService.UpdateTicketAsync(ticket, _companyId, _userId);

				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}


		//Comments

		[HttpGet("GetComments")]
		public async Task<ActionResult<IEnumerable<TicketCommentDTO>>> GetComments([FromQuery] int ticketId)
		{
			try
			{
				IEnumerable<TicketCommentDTO> comments = [];

				if(ticketId != 0)
				{
					comments = await _ticketsService.GetTicketCommentsAsync(ticketId, _companyId);
				}

				return Ok(comments);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpGet("{id:int}/GetComment")]
		public async Task<ActionResult<TicketCommentDTO>> GetCommentById(int commentId)
		{
			try
			{
				TicketCommentDTO? comment = await _ticketsService.GetCommentByIdAsync(commentId, _companyId);

				return comment == null ? NotFound() : Ok(comment);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPost("PostComment")]
		public async Task<ActionResult<TicketCommentDTO>> AddComment([FromBody] TicketCommentDTO comment)
		{
			try
			{
				if (comment.TicketId == 0) return BadRequest();

				string userId = _userId;

				comment.UserId = userId;
				comment.Created = DateTime.UtcNow;

				TicketCommentDTO createdComment = await _ticketsService.AddCommentAsync(comment, _companyId);
				return CreatedAtAction(nameof(GetCommentById), new {id = createdComment.Id }, createdComment);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpPut("{id:int}/UpdateComment")]
		public async Task<ActionResult> UpdateComment([FromRoute] int id, [FromBody] TicketCommentDTO comment)
		{
			if (id != comment.Id) return BadRequest();

			try
			{
				await _ticketsService.UpdateCommentAsync(comment, _companyId, _userId);
				return Ok();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}

		[HttpDelete("{id:int}")]
		public async Task<ActionResult> DeleteComment([FromRoute] int id)
		{
			try
			{
				await _ticketsService.DeleteCommentAsync(id, _companyId);
				return NoContent();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}
		}


		//Ticket Attachments

		[HttpPost("{id}/attachments")]
		public async Task<ActionResult<TicketAttachmentDTO>> PostTicketAttachment(int id,
																			[FromForm] TicketAttachmentDTO attachment,
																			[FromForm] IFormFile? file)
		{
			if (attachment.TicketId != id || file is null)
			{
				return BadRequest();
			}

			var user = await _userManager.GetUserAsync(User);
			var ticket = await _ticketsService.GetTicketByIdAsync(id, user!.CompanyId);

			if (ticket is null)
			{
				return NotFound();
			}

			attachment.UserId = user!.Id;
			attachment.Created = DateTimeOffset.Now;

			if (string.IsNullOrWhiteSpace(attachment.FileName))
			{
				attachment.FileName = file.FileName;
			}

			// ImageHelper was renamed to UploadHelper!
			FileUpload upload = await UploadHelper.GetImageUploadAsync(file);

			try
			{
				var newAttachment = await _ticketsService.AddTicketAttachment(attachment, upload.Data!, upload.Type!, user!.CompanyId);
				return Ok(newAttachment);
			}
			catch
			{
				return Problem();
			}
		}

		// DELETE: api/Tickets/attachments/1
		[HttpDelete("attachments/{attachmentId}")]
		public async Task<IActionResult> DeleteTicketAttachment(int attachmentId)
		{
			var user = await _userManager.GetUserAsync(User);

			await _ticketsService.DeleteTicketAttachment(attachmentId, user!.CompanyId);

			return NoContent();
		}
	}
}

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
	public class TicketsController : ControllerBase
	{
		private readonly ITicketService _ticketsService;

		public TicketsController(ITicketService ticketsService)
		{
			_ticketsService = ticketsService;
		}

		private int _companyId => int.Parse(User.FindFirst("CompanyId")!.Value);

		private string _userId => User.GetUserId()!;

		[HttpPost]
		public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] TicketDTO ticket)
		{
			try
			{

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return Problem();
			}

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
	}

}

using BlazorAuthTemplate.Client;
using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;

namespace BlazorAuthTemplate.Services
{
	public class TicketService : ITicketService
	{

		private readonly ITicketRespository _repository;

		public TicketService(ITicketRespository repository)
		{
			_repository = repository;
		}

		public async Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId)
		{
			Ticket newTicket = new()
			{
				Title = ticket.Title,
				Description = ticket.Description,
				ProjectId = ticket.ProjectId,
				Priority = ticket.Priority,
				Created = DateTimeOffset.Now,
				IsArchived = false,
				IsArchivedByProject = false,
				SubmitterUserId = ticket.SubmitterUserId,
				Type = ticket.Type,
				Status = ticket.Status,
			};

			newTicket = await _repository.AddTicketAsync(newTicket, companyId);

			return newTicket.ToDTO();
		}

		public async Task ArchiveTicketAsync(int ticketId, int companyId)
		{
			await _repository.ArchiveTicketAsync(ticketId, companyId);
		}

		public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
		{
			IEnumerable<Ticket> tickets = await _repository.GetAllTicketsAsync(companyId);

			return tickets.Select(t => t.ToDTO());
		}

		public async Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
		{
			Ticket? ticket = await _repository.GetTicketByIdAsync(ticketId, companyId);

			return ticket?.ToDTO();
		}

		public async Task RestoreTicketAsync(int ticketId, int companyId)
		{
			await _repository.RestoreTicketAsync(ticketId, companyId);
		}

		public async Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId)
		{
			Ticket? originalTicket = await _repository.GetTicketByIdAsync(ticket.Id, companyId);

			if (originalTicket == null) { return; }

			originalTicket.Title = ticket.Title;
			originalTicket.Description = ticket.Description;
			originalTicket.Priority = ticket.Priority;
			originalTicket.Updated = ticket.Updated;
			originalTicket.Type = ticket.Type;

			await _repository.UpdateTicketAsync(originalTicket, companyId, userId);
		}
	}
}

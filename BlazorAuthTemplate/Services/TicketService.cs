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
				IsArchived = ticket.IsArchived,
				IsArchivedByProject = ticket.IsArchivedByProject,
				Type = ticket.Type,
				Status = ticket.Status,
				SubmitterUserId = ticket.SubmitterUserId,
				DeveloperUserId = ticket.DeveloperUserId
			};

			newTicket = await _repository.AddTicketAsync(newTicket, companyId);

			return newTicket.ToDTO();
		}

		public Task ArchiveTicketAsync(int ticketId, int companyId)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
		{
			throw new NotImplementedException();
		}

		public Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
		{
			throw new NotImplementedException();
		}

		public Task RestoreTicketAsync(int ticketId, int companyId)
		{
			throw new NotImplementedException();
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

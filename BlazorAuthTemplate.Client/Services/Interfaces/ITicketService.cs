using BlazorAuthTemplate.Client.Models;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
	public interface ITicketService
	{
		Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId);

		Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId);

		Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId);

		Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId);

		Task ArchiveTicketAsync(int ticketId, int companyId);

		Task RestoreTicketAsync(int ticketId, int companyId);
	}
}

using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;

namespace BlazorAuthTemplate.Client.Services
{
	public class TicketService : ITicketService
	{
		private readonly HttpClient _httpClient;

		public TicketService(HttpClient httpClient)
		{ 
			_httpClient = httpClient;
		}

		public Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId)
		{
			throw new NotImplementedException();
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

		public Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId)
		{
			throw new NotImplementedException();
		}
	}
}

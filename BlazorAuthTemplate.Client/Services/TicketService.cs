using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace BlazorAuthTemplate.Client.Services
{
	public class TicketService : ITicketService
	{
		private readonly HttpClient _httpClient;

		public TicketService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public Task<TicketCommentDTO> AddCommentAsync(TicketCommentDTO comment, int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task<TicketDTO> AddTicketAsync(TicketDTO ticket, int companyId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/tickets", ticket);
				response.EnsureSuccessStatusCode();

				TicketDTO? createdTicket = await response.Content.ReadFromJsonAsync<TicketDTO>();
				return createdTicket!;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task ArchiveTicketAsync(int ticketId, int companyId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/{ticketId}/Archive", ticketId);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public Task DeleteCommentAsync(int commentId, int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
		{
			try
			{
				var tickets = await _httpClient.GetFromJsonAsync<IEnumerable<TicketDTO>>($"api/tickets") ?? [];

				return tickets;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public Task<TicketCommentDTO?> GetCommentByIdAsync(int commentId, int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
		{
			try
			{
				return await _httpClient.GetFromJsonAsync<TicketDTO>($"api/tickets/{ticketId}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId)
		{
			throw new NotImplementedException();
		}

		public async Task RestoreTicketAsync(int ticketId, int companyId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/{ticketId}/Restore", ticketId);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public Task UpdateCommentAsync(TicketCommentDTO comment, int companyId, string userId)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateTicketAsync(TicketDTO ticket, int companyId, string userId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets", ticket);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}

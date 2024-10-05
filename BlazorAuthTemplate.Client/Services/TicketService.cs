using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace BlazorAuthTemplate.Client.Services
{
	public class TicketService : ITicketService
	{
		private readonly HttpClient _httpClient;

		public TicketService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<TicketCommentDTO> AddCommentAsync(TicketCommentDTO comment, int companyId)
		{
			HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/tickets/PostComment", comment);
			response.EnsureSuccessStatusCode();

			TicketCommentDTO? createdComment = await response.Content.ReadFromJsonAsync<TicketCommentDTO>();
			return createdComment!;
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

		public async Task DeleteCommentAsync(int commentId, int companyId)
		{
			HttpResponseMessage response = await _httpClient.DeleteAsync($"api/tickets/{commentId}");
			response.EnsureSuccessStatusCode();
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

		public async Task<TicketCommentDTO?> GetCommentByIdAsync(int commentId, int companyId)
		{
			try
			{
				var comment = await _httpClient.GetFromJsonAsync<TicketCommentDTO>($"api/tickets/{commentId}/GetComment");

				return comment;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
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

		public async Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId)
		{
			try
			{
				var comments = await _httpClient.GetFromJsonAsync<IEnumerable<TicketCommentDTO>>($"api/tickets/GetComments?ticketId={ticketId}") ?? [];
				return comments;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
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

		public async Task UpdateCommentAsync(TicketCommentDTO comment, int companyId, string userId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/{comment.Id}/UpdateComment", comment);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
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

		public async Task<TicketAttachmentDTO> AddTicketAttachment(TicketAttachmentDTO attachment, byte[] uploadData, string contentType, int companyId)
		{
			using var formData = new MultipartFormDataContent();
			formData.Headers.ContentDisposition = new("form-data");

			var fileContent = new ByteArrayContent(uploadData);
			fileContent.Headers.ContentType = new(contentType);

			if (string.IsNullOrWhiteSpace(attachment.FileName))
			{
				formData.Add(fileContent, "file");
			}
			else
			{
				formData.Add(fileContent, "file", attachment.FileName);
			}

			formData.Add(new StringContent(attachment.Id.ToString()), nameof(attachment.Id));
			formData.Add(new StringContent(attachment.FileName ?? string.Empty), nameof(attachment.FileName));
			formData.Add(new StringContent(attachment.Description ?? string.Empty), nameof(attachment.Description));
			formData.Add(new StringContent(DateTimeOffset.Now.ToString()), nameof(attachment.Created));
			formData.Add(new StringContent(attachment.UserId ?? string.Empty), nameof(attachment.UserId));
			formData.Add(new StringContent(attachment.TicketId.ToString()), nameof(attachment.TicketId));

			var res = await _httpClient.PostAsync($"api/tickets/{attachment.TicketId}/attachments", formData);
			res.EnsureSuccessStatusCode();

			var addedAttachment = await res.Content.ReadFromJsonAsync<TicketAttachmentDTO>();
			return addedAttachment!;
		}

		public async Task DeleteTicketAttachment(int attachmentId, int companyId)
		{
			var res = await _httpClient.DeleteAsync($"api/tickets/attachments/{attachmentId}");
			res.EnsureSuccessStatusCode();
		}

		public async Task AddDeveloperToTicket(int projectId, int ticketId, string userId, string managerId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/AddDeveloper/{projectId}/{ticketId}/{userId}", managerId);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task RemoveDeveloperFromTicket(int ticketId, string userId, string managerId)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/tickets/RemoveDeveloper/{ticketId}/{userId}", managerId);
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

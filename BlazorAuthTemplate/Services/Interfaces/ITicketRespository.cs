using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Models;

namespace BlazorAuthTemplate.Services.Interfaces
{
	public interface ITicketRespository
	{
		Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId);

		Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId);

		Task<Ticket> AddTicketAsync(Ticket ticket, int companyId);

		Task<Ticket> UpdateTicketAsync(Ticket ticket, int companyId, string userId);

		Task ArchiveTicketAsync(int ticketId, int companyId);

		Task RestoreTicketAsync(int ticketId, int companyId);


		//Ticket Comments

		Task<IEnumerable<TicketComment>> GetTicketCommentsAsync(int ticketId, int companyId);

		Task<TicketComment?> GetCommentByIdAsync(int commentId, int companyId);

		Task<TicketComment> AddCommentAsync(TicketComment comment, int companyId);

		Task DeleteCommentAsync(int commentId, int companyId);

		Task UpdateCommentAsync(TicketComment comment, int companyId, string userId);


		//Ticket Attachments

		Task<TicketAttachment> AddTicketAttachment(TicketAttachment attachment, int companyId);

		Task DeleteTicketAttachment(int attachmentId, int companyId);

		Task AddDeveloperToTicket(int projectId, int ticketId, string userId, string managerId);

		Task RemoveDeveloperFromTicket(int ticketId, string userId, string managerId);
	}
}

using BlazorAuthTemplate.Client;
using BlazorAuthTemplate.Client.Helpers;
using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.IdentityModel.Abstractions;

namespace BlazorAuthTemplate.Services
{
	public class TicketService : ITicketService
	{

		private readonly ITicketRespository _repository;

		public TicketService(ITicketRespository repository)
		{
			_repository = repository;
		}

		public async Task<TicketCommentDTO> AddCommentAsync(TicketCommentDTO comment, int companyId)
		{
			TicketComment newComment = new()
			{
				Id = comment.Id,
				Content = comment.Content,
				Created = DateTimeOffset.Now,
				TicketId = comment.TicketId,
				UserId = comment.UserId
			};

			newComment = await _repository.AddCommentAsync(newComment, companyId);

			return newComment.ToDTO();
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

		public async Task DeleteCommentAsync(int commentId, int companyId)
		{
			await _repository.DeleteCommentAsync(commentId, companyId);
		}

		public async Task<IEnumerable<TicketDTO>> GetAllTicketsAsync(int companyId)
		{
			IEnumerable<Ticket> tickets = await _repository.GetAllTicketsAsync(companyId);

			return tickets.Select(t => t.ToDTO());
		}

		public async Task<TicketCommentDTO?> GetCommentByIdAsync(int commentId, int companyId)
		{
			TicketComment? comment = await _repository.GetCommentByIdAsync(commentId, companyId);

			return comment?.ToDTO();
		}

		public async Task<TicketDTO?> GetTicketByIdAsync(int ticketId, int companyId)
		{
			Ticket? ticket = await _repository.GetTicketByIdAsync(ticketId, companyId);

			return ticket?.ToDTO();
		}

		public async Task<IEnumerable<TicketCommentDTO>> GetTicketCommentsAsync(int ticketId, int companyId)
		{
			IEnumerable<TicketComment> comments = await _repository.GetTicketCommentsAsync(ticketId, companyId);

			return comments.Select(t => t.ToDTO());
		}

		public async Task RestoreTicketAsync(int ticketId, int companyId)
		{
			await _repository.RestoreTicketAsync(ticketId, companyId);
		}

		public async Task UpdateCommentAsync(TicketCommentDTO commentDTO, int companyId, string userId)
		{
			TicketComment? updatedComment = await _repository.GetCommentByIdAsync(commentDTO.Id, companyId);

			if (updatedComment is not null)
			{
				updatedComment.Content = commentDTO.Content;

				await _repository.UpdateCommentAsync(updatedComment, companyId, userId);
			}
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

		public async Task<TicketAttachmentDTO> AddTicketAttachment(TicketAttachmentDTO attachment, byte[] uploadData, string contentType, int companyId)
		{
			FileUpload file = new()
			{
				Type = contentType,
				Data = uploadData,
			};

			TicketAttachment dbAttachment = new()
			{
				TicketId = attachment.TicketId,
				Description = attachment.Description,
				FileName = attachment.FileName,
				FileUpload = file,
				Created = DateTimeOffset.Now,
				UserId = attachment.UserId
			};

			dbAttachment = await _repository.AddTicketAttachment(dbAttachment, companyId);

			return dbAttachment.ToDTO();
		}

		public async Task DeleteTicketAttachment(int attachmentId, int companyId)
		{
			await _repository.DeleteTicketAttachment(attachmentId, companyId);
		}
	}
}

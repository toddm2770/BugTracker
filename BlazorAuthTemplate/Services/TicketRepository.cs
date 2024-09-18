using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace BlazorAuthTemplate.Services
{
	public class TicketRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ITicketRespository
	{
		public async Task<TicketComment> AddCommentAsync(TicketComment comment, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			context.TicketComments.Add(comment);
			await context.SaveChangesAsync();

			return comment;
		}

		public async Task<Ticket> AddTicketAsync(Ticket ticket, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			ticket.Created = DateTimeOffset.Now;

			context.Tickets.Add(ticket);

			try
			{
				await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}

			return ticket;
		}

		public async Task ArchiveTicketAsync(int ticketId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Ticket? ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

			if (ticket != null)
			{
				ticket.IsArchived = true;
				await context.SaveChangesAsync();
			}
		}

		public async Task DeleteCommentAsync(int commentId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			TicketComment? comment = await context.TicketComments.FirstOrDefaultAsync(t => t.Id == commentId);

			if (comment != null)
			{
				context.TicketComments.Remove(comment);
				await context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			List<Ticket> tickets = await context.Tickets
												.Include(t => t.TicketComments)
												.ThenInclude(t => t.User)
												.ToListAsync();
			return tickets;
		}

		public async Task<TicketComment?> GetCommentByIdAsync(int commentId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			TicketComment? comment = await context.TicketComments.Include(t => t.User)
																 .FirstOrDefaultAsync(t => t.Id == commentId);

			return comment;
		}

		public async Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Ticket? ticket = await context.Tickets
										  .FirstOrDefaultAsync(t => t.Id == ticketId);
			return ticket;
		}

		public async Task<IEnumerable<TicketComment>> GetTicketCommentsAsync(int ticketId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			List<TicketComment> comments = await context.TicketComments
														.Where(t => t.TicketId == ticketId)
														.Include(t => t.User)
														.ToListAsync();
			return comments;
		}

		public async Task RestoreTicketAsync(int ticketId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Ticket? ticket = await context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

			if (ticket != null)
			{
				ticket.IsArchived = false;
				await context.SaveChangesAsync();
			}
		}

		public async Task UpdateCommentAsync(TicketComment comment, int companyId, string userId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			if(await context.TicketComments.AnyAsync(c => c.Id == comment.Id))
			{
				context.TicketComments.Update(comment);
				await context.SaveChangesAsync();
			}
		}

		public async Task<Ticket> UpdateTicketAsync(Ticket ticket, int companyId, string userId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			context.Update(ticket);
			await context.SaveChangesAsync();

			return ticket;
		}

		public async Task<TicketAttachment> AddTicketAttachment(TicketAttachment attachment, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			// make sure the ticket exists and belongs to this company
			var ticket = await context.Tickets
									  .FirstOrDefaultAsync(t => t.Id == attachment.TicketId && t.Project!.CompanyId == companyId);

			// save it if it does
			if (ticket is not null)
			{
				attachment.Created = DateTimeOffset.Now;
				context.TicketAttachments.Add(attachment);
				await context.SaveChangesAsync();

				return attachment;
			}
			else
			{
				throw new ArgumentException("Ticket not found");
			}
		}

		public async Task DeleteTicketAttachment(int attachmentId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			var attachment = await context.TicketAttachments
										  .Include(a => a.FileUpload)
										  .FirstOrDefaultAsync(a => a.Id == attachmentId && a.Ticket!.Project!.CompanyId == companyId);

			if (attachment is not null)
			{
				context.Remove(attachment);
				context.Remove(attachment.FileUpload!);
				await context.SaveChangesAsync();
			}
		}
	}
}

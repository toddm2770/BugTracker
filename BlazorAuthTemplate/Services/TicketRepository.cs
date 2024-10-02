using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using static BlazorAuthTemplate.Models.Enums;

namespace BlazorAuthTemplate.Services
{
	public class TicketRepository(IDbContextFactory<ApplicationDbContext> contextFactory, IServiceProvider svcProvider) : ITicketRespository
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
										  .Include(t => t.SubmitterUser)
										  .Include(t => t.DeveloperUser)
										  .Include(t => t.TicketAttachments)
										  .Include(t => t.TicketComments)
										  .FirstOrDefaultAsync(t => t.Id == ticketId);

			Console.WriteLine($"Updated: {ticket?.Updated?.ToString() ?? "null"}");
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

			if (await context.TicketComments.AnyAsync(c => c.Id == comment.Id))
			{
				context.TicketComments.Update(comment);
				await context.SaveChangesAsync();
			}
		}

		public async Task<Ticket> UpdateTicketAsync(Ticket ticket, int companyId, string userId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			ticket.Updated = DateTimeOffset.Now;

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

		public async Task AddDeveloperToTicket(int projectId, int ticketId, string userId, string managerId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
			if (manager == null) return;

			bool isAdmin = await userManager.IsInRoleAsync(manager, nameof(Roles.Admin));
			bool isProjectManager = await userManager.IsInRoleAsync(manager, nameof(Roles.ProjectManager));

			if (!isAdmin && !isProjectManager) return;

			ApplicationUser? userToAdd = await context.Users
				.FirstOrDefaultAsync(u => u.Id == userId && u.CompanyId == manager.CompanyId);

			if (userToAdd == null) return;

			bool userIsAdmin = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.Admin));
			bool userIsProjectManager = await userManager.IsInRoleAsync(userToAdd, nameof(Roles.ProjectManager));

			if (userIsAdmin || userIsProjectManager) return;

			Ticket? ticket = await context.Tickets
				.Include(t => t.DeveloperUser)
				.Include(t => t.Project)
				.FirstOrDefaultAsync(t => t.Id == ticketId && t.Project != null && t.Project.CompanyId == manager.CompanyId);

			if (ticket == null) return;

			if (ticket.DeveloperUser != null)
			{
				await RemoveDeveloperFromTicket(ticket.Id, userId, managerId);
			}

			ticket.DeveloperUser = userToAdd;
			await context.SaveChangesAsync();
		}

		public async Task RemoveDeveloperFromTicket(int ticketId, string userId, string managerId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();
			using IServiceScope scope = svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			ApplicationUser? manager = await userManager.FindByIdAsync(managerId);
			if (manager == null) return;

			bool isAdmin = await userManager.IsInRoleAsync(manager, nameof(Roles.Admin));
			bool isProjectManager = await userManager.IsInRoleAsync(manager, nameof(Roles.ProjectManager));

			if (!isAdmin && !isProjectManager) return;

			Ticket? ticket = await context.Tickets
				.Include(t => t.DeveloperUser)
				.FirstOrDefaultAsync(t => t.Id == ticketId);

			if (ticket != null && ticket.DeveloperUser != null)
			{
				ticket.DeveloperUser = null;
				await context.SaveChangesAsync();
			}
		}
	}
}

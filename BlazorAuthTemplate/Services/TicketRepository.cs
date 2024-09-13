using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuthTemplate.Services
{
	public class TicketRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : ITicketRespository
	{
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

			if(ticket != null)
			{
				ticket.IsArchived = true;
				await context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			List<Ticket> tickets = await context.Tickets
												.ToListAsync();
			return tickets;
		}

		public async Task<Ticket?> GetTicketByIdAsync(int ticketId, int companyId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			Ticket? ticket = await context.Tickets
										  .FirstOrDefaultAsync(t => t.Id == ticketId);
			return ticket;
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

		public async Task<Ticket> UpdateTicketAsync(Ticket ticket, int companyId, string userId)
		{
			using ApplicationDbContext context = contextFactory.CreateDbContext();

			context.Update(ticket);
			await context.SaveChangesAsync();

			return ticket;
		}
	}
}

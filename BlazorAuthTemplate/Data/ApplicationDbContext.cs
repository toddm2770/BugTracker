using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorAuthTemplate.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<FileUpload> Uploads { get; set; }
        
        public override DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Invite> Invites { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }
    }
}

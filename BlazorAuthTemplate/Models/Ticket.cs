using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class Ticket
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;

        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

       [Required]
       public string? Description { get; set; }

        public DateTimeOffset Created 
        {
            get => _created; 
            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset? Updated
        {
            get => _updated;
            set => _updated = value?.ToUniversalTime();
        }

        public bool IsArchived { get; set; }

        public bool IsArchivedByProject { get; set; }

        [Required]
        public Enums.TicketPriority Priority { get; set; }

        [Required]
        public Enums.TicketType TicketType { get; set; }

        [Required]
        public Enums.TicketStatus TicketStatus { get; set; }

        [Required]
        public virtual Project? Project { get; set; }

        [Required]
        public virtual ApplicationUser? SubmitterUser { get; set; }

        public virtual ApplicationUser? DeveloperUser { get; set; }

        public virtual ICollection<TicketComment>? TicketComments { get; set; } = [];

        public virtual ICollection<TicketAttachment>? TicketAttachments { get; set; } = [];
    }

    public static class TicketExtension
    {
        public static TicketDTO ToDTO(this Ticket ticket)
        {
            return new TicketDTO()
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Created = ticket.Created,
                Updated = ticket.Updated,
                IsArchived = ticket.IsArchived,
                IsArchivedByProject = ticket.IsArchivedByProject,
                Priority = ticket.Priority,
                TicketType = ticket.TicketType,
                TicketStatus = ticket.TicketStatus,
                Project = ticket.Project?.ToDTO(),
                SubmitterUser = ticket.SubmitterUser?.ToDTO(),
                DeveloperUser = ticket.DeveloperUser?.ToDTO(),
                TicketComments = [.. ticket.TicketComments?.Select(c => c.ToDTO())],
                TicketAttachments = [.. ticket.TicketAttachments?.Select(a => a.ToDTO())]
            };
        }
    }
}

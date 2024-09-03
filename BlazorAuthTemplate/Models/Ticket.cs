using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using System.ComponentModel.DataAnnotations;
using static BlazorAuthTemplate.Models.Enums;

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

        public TicketPriority Priority { get; set; }

        public TicketType Type { get; set; }

        public TicketStatus Status { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public virtual Project? Project { get; set; }

		[Required]
		public string? SubmitterUserId { get; set; }

		public virtual ApplicationUser? SubmitterUser { get; set; }

		public string? DeveloperUserId { get; set; }

		public virtual ApplicationUser? DeveloperUser { get; set; }

        public virtual ICollection<TicketComment>? TicketComments { get; set; } = [];

        public virtual ICollection<TicketAttachment>? TicketAttachments { get; set; } = [];
    }

    public static class TicketExtension
    {
        public static TicketDTO ToDTO(this Ticket ticket)
        {

            if (ticket.Project != null) 
            {
                ticket.Project.Tickets = [];
            }

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
                Type = ticket.Type,
                Status = ticket.Status,
                Project = ticket.Project?.ToDTO(),
                ProjectId = ticket.ProjectId,
                SubmitterUser = ticket.SubmitterUser?.ToDTO(),
                SubmitterUserId = ticket.SubmitterUserId,
                DeveloperUser = ticket.DeveloperUser?.ToDTO(),
                DeveloperUserId = ticket.DeveloperUserId,
                TicketComments = [.. ticket.TicketComments?.Select(c => c.ToDTO())],
                TicketAttachments = [.. ticket.TicketAttachments?.Select(a => a.ToDTO())]
            };
        }
    }
}

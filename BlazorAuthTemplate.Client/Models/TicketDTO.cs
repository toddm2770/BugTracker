using BlazorAuthTemplate.Models;
using System.ComponentModel.DataAnnotations;
using static BlazorAuthTemplate.Models.Enums;

namespace BlazorAuthTemplate.Client.Models
{
    public class TicketDTO
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
            set => value?.ToUniversalTime();
        }

        public bool IsArchived { get; set; }

        public bool IsArchivedByProject { get; set; }

        public TicketPriority Priority { get; set; }

        public TicketType Type { get; set; }

        public TicketStatus Status { get; set; }

        public int ProjectId { get; set; }

        public ProjectDTO? Project { get; set; }

        [Required]
        public string? SubmitterUserId { get; set; }

        public UserDTO? SubmitterUser { get; set; }

        public string? DeveloperUserId { get; set; }

        public UserDTO? DeveloperUser { get; set; }

        public ICollection<TicketCommentDTO>? TicketComments { get; set; }

        public ICollection<TicketAttachmentDTO>? TicketAttachments { get; set; }
    }
}

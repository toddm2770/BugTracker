using BlazorAuthTemplate.Models;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public Enums.TicketPriority Priority { get; set; }

        [Required]
        public Enums.TicketType TicketType { get; set; }

        [Required]
        public Enums.TicketStatus TicketStatus { get; set; }

        [Required]
        public ProjectDTO? Project { get; set; }

        [Required]
        public UserDTO? SubmitterUser { get; set; }

        public UserDTO? DeveloperUser { get; set; }

        public ICollection<TicketCommentDTO>? TicketComments { get; set; }

        public ICollection<TicketAttachmentDTO>? TicketAttachments { get; set; }
    }
}

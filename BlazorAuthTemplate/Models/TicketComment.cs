using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class TicketComment
    {
        private DateTimeOffset _created;

        public int Id { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTimeOffset Created 
        { get => _created;
          set => _created = value.ToUniversalTime(); 
        }

        public int TicketId { get; set; }

        public Ticket? Ticket { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }
    }

    public static class TicketCommentExtension
    {
        public static TicketCommentDTO ToDTO(this TicketComment ticketComment)
        {
                return new TicketCommentDTO()
                {
                    Id = ticketComment.Id,
                    Content = ticketComment.Content,
                    Created = ticketComment.Created,
                    TicketId = ticketComment.TicketId,
                    User = ticketComment.User?.ToDTO(),
                    UserId = ticketComment.User?.Id
                };
        }
    }
}

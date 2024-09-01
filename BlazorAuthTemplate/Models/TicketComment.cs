using BlazorAuthTemplate.Client.Models;
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

        public Ticket? Ticket { get; set; }

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
                    TicketId = ticketComment.Ticket!.Id,
                    User = ticketComment.User?.ToDTO(),
                    UserId = ticketComment.User?.Id
                };
        }
    }
}

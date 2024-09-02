using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class TicketAttachment
    {
        private DateTimeOffset _Created;

        public int Id { get; set; }

        [Required]
        public string? FileName { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset Created 
        { get => _Created; 
          set => _Created = value.ToUniversalTime(); 
        }

        public FileUpload? FileUpload { get; set; }

        public ApplicationUser? User { get; set; }

        public Ticket? Ticket { get; set; }
    }

    public static class TicketAttachmentExtension
    {
        public static TicketAttachmentDTO ToDTO(this TicketAttachment ticketAttachment)
        {
            return new TicketAttachmentDTO()
            {
                Id = ticketAttachment.Id,
                FileName = ticketAttachment.FileName,
                Description = ticketAttachment.Description,
                Created = ticketAttachment.Created,
                AttachmentURL = ticketAttachment.FileUpload?.Extension,
                UserId = ticketAttachment.User?.Id,
                User = ticketAttachment.User?.ToDTO(),
                TicketId = ticketAttachment.Ticket!.Id
            };
        }
    }
}

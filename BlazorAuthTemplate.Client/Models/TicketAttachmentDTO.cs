﻿namespace BlazorAuthTemplate.Client.Models
{
    public class TicketAttachmentDTO
    {
        private DateTimeOffset _created;

        public int Id { get; set; }

        public string? FileName { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset Created 
        { get => _created; 
          set => _created = value.ToUniversalTime(); 
        }

        public string? AttachmentURL { get; set; }

        public string? UserId { get; set; }

        public UserDTO? User { get; set; }

        public int TicketId { get; set; }
    }
}

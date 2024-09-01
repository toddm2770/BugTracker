﻿using BlazorAuthTemplate.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Client.Models
{
    public class ProjectDTO
    {
        public DateTimeOffset _created;
        public DateTimeOffset _startDate;
        public DateTimeOffset _endDate;
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        public DateTimeOffset StartDate
        {
            get => _startDate;
            set => _startDate = value.ToUniversalTime();
        }

        public DateTimeOffset EndDate
        {
            get => _endDate;
            set => _endDate = value.ToUniversalTime();
        }

        [Required]
        public Enums.ProjectPriority Priority { get; set; }

        public bool IsArchived { get; set; }

        public ICollection<UserDTO>? Members { get; set; } = [];

        public ICollection<TicketDTO>? Tickets { get; set; } = [];
    }
}

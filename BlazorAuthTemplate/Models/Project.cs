using BlazorAuthTemplate.Client.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class Project
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

        public virtual Company? Company { get; set; }

        public virtual ICollection<ApplicationUser>? Members { get; set; } = [];

        public virtual ICollection<Ticket>? Tickets { get; set; } = [];


    }

    public static class ProjectExtension
    {
        public static ProjectDTO ToDTO(this Project project)
        {
            return new ProjectDTO()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Created = project.Created,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Priority = project.Priority,
                IsArchived = project.IsArchived,
                Members = [.. project.Members?.Select(m => m.ToDTO())],
                Tickets = [.. project.Tickets?.Select(t => t.ToDTO())]

            };
        }
    }
}

//[.. company.Projects.Select(p => p.ToDTO())]

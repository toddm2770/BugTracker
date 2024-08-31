﻿using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Client.Models
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public ICollection<ProjectDTO> Projects { get; set; } = new List<ProjectDTO>();

        public ICollection<UserDTO> Members { get; set; } = new List<UserDTO>();

        public ICollection<InviteDTO> Invites { get; set; } = new List<InviteDTO>();
    }
}

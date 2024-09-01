using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Client.Models
{
    public class CompanyDTO
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? CompanyImageUrl { get; set; }

        public ICollection<ProjectDTO> Projects { get; set; } = [];

        public ICollection<UserDTO> Members { get; set; } = [];

        public ICollection<InviteDTO> Invites { get; set; } = [];
    }
}

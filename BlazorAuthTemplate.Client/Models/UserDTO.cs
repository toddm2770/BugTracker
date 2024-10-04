using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Client.Models
{
    public class UserDTO
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string? ImageUrl { get; set; }

        [Required]
        public string? Email { get; set; }

        public string? Role { get; set; }
	}
}

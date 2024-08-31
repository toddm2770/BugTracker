using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Client.Models
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? FullName { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}

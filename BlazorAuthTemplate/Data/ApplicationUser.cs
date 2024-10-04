using BlazorAuthTemplate.Client;
using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Data
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? FullName => $"{FirstName} {LastName}";

        public Guid? ImageId { get; set; }
        public virtual FileUpload? Image { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; } = new Company();

        public ICollection<Project>? Projects { get; set; }

        public virtual ICollection<TicketComment>? Comments { get; set; }
    }

	public static class ApplicationUserExtension
	{
		public static UserDTO ToDTO(this ApplicationUser applicationUser)
		{
			return new UserDTO()
			{
                Id = applicationUser.Id,
				FirstName = applicationUser.FirstName,
				LastName = applicationUser.LastName,
				Email = applicationUser.Email,
                CompanyId = applicationUser.CompanyId,
			};
		}
	}
}

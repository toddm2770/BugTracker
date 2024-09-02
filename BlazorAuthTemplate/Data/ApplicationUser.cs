using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
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

        public virtual Company? Company { get; set; }

        ICollection<Project>? Projects { get; set; }
    }

	public static class ApplicationUserExtension
	{
		public static UserDTO ToDTO(this ApplicationUser applicationUser)
		{
			return new UserDTO()
			{
				FirstName = applicationUser.FirstName,
				LastName = applicationUser.LastName,
				FullName = applicationUser.FullName,
				ImageUrl = applicationUser.Image?.Extension,
				Email = applicationUser.Email
			};
		}
	}

}

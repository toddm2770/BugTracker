using BlazorAuthTemplate.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorAuthTemplate.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public Guid ProfilePictureId { get; set; }

        public virtual FileUpload? ProfilePicture { get; set; }

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
                ImageUrl = applicationUser.ProfilePicture?.Extension,
                Email = applicationUser.Email
            };
        }
    }
}

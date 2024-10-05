using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Helpers;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

		public Guid? ImageId { get; set; }

        public virtual FileUpload? Image { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        public virtual ICollection<ApplicationUser> Members { get; set; } = new List<ApplicationUser>();

        public virtual ICollection<Invite> Invites { get; set; } = new List<Invite>();
    }

    public static class CompanyExtension
    {
        public static CompanyDTO ToDTO(this Company company)
        {
            return new CompanyDTO()
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                CompanyImageUrl = company.ImageId.HasValue ? $"api/uploads/{company.ImageId}" : UploadHelper.DefaultCompanyImage,
                Projects = [.. company.Projects.Select(p => p.ToDTO())],
                Members = [.. company.Members.Select(p => p.ToDTO())],
                Invites = [.. company.Invites.Select(p => p.ToDTO())]
            };
        }
    }
}

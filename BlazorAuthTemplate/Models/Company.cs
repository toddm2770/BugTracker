using BlazorAuthTemplate.Client.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public FileUpload? CompanyImage { get; set; }

        public virtual FileUpload? Image { get; set; }

        public string? CompanyImageURL { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = [];

        public virtual ICollection<ApplicationUser> Members { get; set; } = [];

        public virtual ICollection<Invite> Invites { get; set; } = [];
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
                CompanyImageUrl = company.CompanyImage?.Extension,
                Projects = [.. company.Projects.Select(p => p.ToDTO())],
                Members = [.. company.Members.Select(p => p.ToDTO())],
                Invites = [.. company.Invites.Select(p => p.ToDTO())]
            };
        }
    }
}

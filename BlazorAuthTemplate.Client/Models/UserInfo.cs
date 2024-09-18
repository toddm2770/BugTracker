using BlazorAuthTemplate.Client.Models;

namespace BlazorAuthTemplate.Client
{

    public class UserInfo
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ProfilePictureUrl { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string[]? Roles { get; set; }
        public required int CompanyId { get; set; }
    }
}

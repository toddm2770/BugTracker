using BlazorAuthTemplate.Client.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorAuthTemplate.Models
{
    public class Invite
    {
        private DateTimeOffset _inviteDate;
        private DateTimeOffset _joinDate;
        public int Id { get; set; }

        public DateTimeOffset InviteDate
        {
            get => _inviteDate;
            set => _inviteDate = value.ToUniversalTime();
        }

        public DateTimeOffset JoinDate
        {
            get => _joinDate;
            set => _joinDate = value.ToUniversalTime();
        }

        public Guid CompanyToken { get; set; }

        [Required]
        public string? InviteeEmail { get; set; }

        [Required]
        public string? InviteeFirstName { get; set; }

        [Required]
        public string? InviteeLastName { get; set; }

        public string? Message { get; set; }

        public bool IsValid { get; set; }

        public virtual Company? Company { get; set; }

        public int ProjectId { get; set; }

        public virtual Project? Project { get; set; }

        public virtual ApplicationUser? Invitor { get; set; }

        public virtual ApplicationUser? Invitee { get; set; }
    }

    public static class InviteExtension
    {
        public static InviteDTO ToDTO(this Invite invite)
        {
            return new InviteDTO()
            {
                Id = invite.Id,
                InviteDate = invite.InviteDate,
                JoinDate = invite.JoinDate,
                InviteeEmail = invite.InviteeEmail,
                InviteeFirstName = invite.InviteeFirstName,
                InviteeLastName = invite.InviteeLastName,
                Message = invite.Message,
                IsValid = invite.IsValid,
                ProjectId = invite.ProjectId,
                InviteProject = invite.Project?.ToDTO(),
                InviteeId = invite.Invitee?.Id,
                Invitee = invite.Invitee?.ToDTO(),
                InvitorId = invite.Invitor?.Id,
                Invitor = invite.Invitor?.ToDTO()
            };
        }
    }
}

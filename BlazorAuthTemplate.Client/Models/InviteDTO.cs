namespace BlazorAuthTemplate.Client.Models
{
    public class InviteDTO
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


    }
}

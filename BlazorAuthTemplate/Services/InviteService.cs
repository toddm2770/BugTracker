using BlazorAuthTemplate.Client.Models;
using BlazorAuthTemplate.Client.Services.Interfaces;
using BlazorAuthTemplate.Models;
using BlazorAuthTemplate.Services.Interfaces;

namespace BlazorAuthTemplate.Services
{
	public class InviteDTOService(IInviteRepository inviteRepository) : IInviteDTOService
	{
		public async Task<InviteDTO> AddInviteAsync(InviteDTO invite)
		{
			// save the invite
			var createdInvite = await inviteRepository.CreateInvite(
				invite.InviteeEmail!,
				invite.InviteeFirstName!,
				invite.InviteeLastName!,
				invite.Message!,
				invite.ProjectId,
				invite.InvitorId!);

			// send it right away
			await inviteRepository.SendInvite(createdInvite.Id, createdInvite.CompanyId);

			return createdInvite.ToDTO();
		}
	}
}

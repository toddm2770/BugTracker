using BlazorAuthTemplate.Client.Models;

namespace BlazorAuthTemplate.Client.Services.Interfaces
{
	public interface IInviteDTOService
	{
		Task<InviteDTO> AddInviteAsync(InviteDTO invite);
	}
}

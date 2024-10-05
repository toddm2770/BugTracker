using BlazorAuthTemplate.Data;
using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using static BlazorAuthTemplate.Models.Enums;

namespace BlazorAuthTemplate.Services.Interfaces
{
	public class InviteRepository : IInviteRepository
	{
		private readonly IDataProtector _protector;
		private readonly IEmailSender _emailSender;
		private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
		private readonly IServiceProvider _svcProvider;
		private readonly NavigationManager _navigationManager;

		public InviteRepository(IDataProtectionProvider protectionProvider, // allows for encryption/decryption of sensitive data
								IConfiguration config, // allows for access to the key used for encryption/decryption
								IEmailSender emailSender, // used to send invites via email
								IDbContextFactory<ApplicationDbContext> contextFactory, // used to access the database
								IServiceProvider svcProvider, // used to access the UserManager
								NavigationManager navigationManager) // used to generate URLs for the invite
		{
			string? protectionPurpose = config["InviteEncryptionKey"] ?? Environment.GetEnvironmentVariable("InviteEncryptionKey");

			if (string.IsNullOrEmpty(protectionPurpose))
			{
				throw new Exception("No InviteEncryptionKey found!");
			}

			// creates an instance of an IDataProtector using our encryption key
			_protector = protectionProvider.CreateProtector(protectionPurpose);

			// assign the other injected dependencies...
			_emailSender = emailSender;
			_contextFactory = contextFactory;
			_svcProvider = svcProvider;
			_navigationManager = navigationManager;
		}

		public async Task AcceptInviteAsync(int inviteId, string inviteeId)
		{
			using ApplicationDbContext context = _contextFactory.CreateDbContext();

			// look up the invite
			Invite? invite = await context.Invites.FirstOrDefaultAsync(i => i.Id == inviteId && i.IsValid == true);

			if (invite is not null)
			{
				// invalidate the invite and join the invitee's account to the invite
				invite.InviteeId = inviteeId;
				invite.IsValid = false;
				invite.JoinDate = DateTime.Now;

				await context.SaveChangesAsync();
			}
		}

		public async Task<Invite> CreateInvite(string inviteeEmail, string inviteeFirstName, string inviteeLastName, string message, int projectId, string invitorId)
		{
			using ApplicationDbContext context = _contextFactory.CreateDbContext();
			using var scope = _svcProvider.CreateScope();
			UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// look up the user sending the invite
			ApplicationUser? invitor = await userManager.FindByIdAsync(invitorId);

			if (invitor is null)
			{
				throw new Exception("User not found for invite");
			}

			// make sure the user sending the invite is an admin
			bool isAdmin = await userManager.IsInRoleAsync(invitor, nameof(Roles.Admin));

			if (isAdmin == false)
			{
				throw new Exception("User is not authorized to create invites");
			}

			// make sure the project exists and belongs to the company of the invitor
			Project? project = await context.Projects.FirstOrDefaultAsync(p => p.Id == projectId && p.CompanyId == invitor.CompanyId);

			if (project is null)
			{
				throw new Exception("Project for invite not found");
			}

			// make sure the user being invited doesn't already exist
			var invitedUser = await userManager.FindByEmailAsync(inviteeEmail);

			if (invitedUser is not null)
			{
				throw new Exception("User already exists!");
			}

			// create the invite
			Invite invite = new()
			{
				InviteDate = DateTime.Now,
				CompanyToken = Guid.NewGuid(),
				InviteeEmail = inviteeEmail,
				InviteeFirstName = inviteeFirstName,
				InviteeLastName = inviteeLastName,
				Message = message,
				IsValid = true,
				CompanyId = invitor.CompanyId,
				ProjectId = projectId,
				InvitorId = invitorId,
			};

			// store it in the database
			context.Invites.Add(invite);
			await context.SaveChangesAsync();

			return invite;
		}

		public async Task<Invite?> GetValidInviteAsync(string protectedToken, string protectedEmail, string protectedCompanyId)
		{
			// this will decrypt the encrypted companyId using the key we set up in the constructor
			string companyIdAsString = _protector.Unprotect(protectedCompanyId);

			// int.TryParse attempts to parse a value and returns true if it was successful.
			// If it was, the companyId variable will be assigned the parsed value.
			if (int.TryParse(companyIdAsString, out int companyId))
			{
				// decrypt the token and email
				Guid token = Guid.Parse(_protector.Unprotect(protectedToken));
				string email = _protector.Unprotect(protectedEmail);

				using ApplicationDbContext context = _contextFactory.CreateDbContext();

				// look up the invite based on the token, email, and companyId
				Invite? invite = await context.Invites
					.Include(i => i.Invitor)
					.Include(i => i.Project)
					.Include(i => i.Company)
					.FirstOrDefaultAsync(i => i.CompanyToken == token && i.CompanyId == companyId && i.InviteeEmail == email && i.IsValid == true);

				if (invite is null) return null;

				// if it's been over a week, the invite is no longer valid
				if (invite.InviteDate.AddDays(7) < DateTime.Now)
				{
					invite.IsValid = false;
					await context.SaveChangesAsync();
					return null;
				}

				// if we made it this far, the invite is valid!
				return invite;
			}

			return null;
		}

		public async Task<bool> SendInvite(int inviteId, int companyId)
		{
			using ApplicationDbContext context = _contextFactory.CreateDbContext();

			bool success = false;

			try
			{
				// look up the invite
				Invite? invite = await context.Invites
					.Include(i => i.Company)
					.Include(i => i.Project)
					.Include(i => i.Invitor)
					.FirstOrDefaultAsync(i => i.Id == inviteId && i.CompanyId == companyId);

				if (invite is not null)
				{
					// encrypt the token, email, and companyId. This will allow us to send it via URL and prevent malicious users
					// from tampering with the values or attempting to accept an invitation they weren't sent.
					string protectedToken = _protector.Protect(invite.CompanyToken.ToString()); // a random GUID to make it impossible to spoof invite codes
					string protectedEmail = _protector.Protect(invite.InviteeEmail!); // the email of the person being invited
					string protectedCompanyId = _protector.Protect(invite.CompanyId.ToString()); // the company ID the invite belongs to

					Dictionary<string, object?> inviteQueryParams = new Dictionary<string, object?>()
				{
					{ "token", protectedToken },
					{ "email", protectedEmail },
					{ "company", protectedCompanyId },
				};

					// create the "/Account/Register/Invite" URL with the encrypted values as query parameters
					string inviteUrl = _navigationManager.GetUriWithQueryParameters("/Account/Register/Invite", inviteQueryParams);
					// convert the URL to an absolute URL, which puts our host name in front of the relative URL
					// e.g. "/Account/Register/Invite?token=..." becomes "https://mywebsite.com/Account/Register/Invite?token=..."
					string callbackUrl = _navigationManager.ToAbsoluteUri(inviteUrl).ToString();

					// CHANGE THIS to match your bug tracker's branding!
					string subject = $"Invite to join Olympus Bug Tracker with {invite.Company!.Name}";

					// CHANGE THIS to match your bug tracker's branding!
					// you can customize this however you like, just know that 
					// Bootstrap is not available in emails. The callback URL is
					// the link the user will click to accept the invite.
					string message =
						$"""
                    You've been invited by {invite.Invitor!.FullName} to
                    join the Olympus Bug Tracker for {invite.Company!.Name}
                    on the project {invite.Project!.Name}.
                    <blockquote>
                        {invite.Message}
                    </blockquote>
                    <a href="{callbackUrl}">Click here</a> to accept this invite and register your account.
                    <br><br>
                    <small>If you cannot click the link above, copy and paste the following URL in your address bar</small>
                    <br>
                    <small>{callbackUrl}</small>
                    """;

					await _emailSender.SendEmailAsync(invite.InviteeEmail!, subject, message);

					success = true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error sending invite");
				Console.WriteLine(ex.Message);
				success = false;
			}

			return success;
		}
	}
}

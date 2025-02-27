﻿using BlazorAuthTemplate.Client.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorAuthTemplate.Client.Helpers
{
    public static class UserInfoHelper
    {
        public static UserInfo? GetUserInfo(AuthenticationState authState)
        {
            string? userId = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? email = authState.User.FindFirst(ClaimTypes.Email)?.Value;
            string? firstName = authState.User.FindFirst(nameof(UserInfo.FirstName))?.Value;
            string? lastName = authState.User.FindFirst(nameof(UserInfo.LastName))?.Value;
            string? profilePictureUrl = authState.User.FindFirst(nameof(UserInfo.ProfilePictureUrl))?.Value;
            string[]? roles = authState.User.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToArray();
            int companyId = 0;

			if (authState.User.FindFirst("CompanyId") is { }  companyIdClaim)
			{
                int.TryParse(companyIdClaim.Value, out companyId);
			}

			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName)
                || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(profilePictureUrl))
            {
                return null;
            }

            UserInfo userInfo = new UserInfo()
            {
                UserId = userId,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                ProfilePictureUrl = profilePictureUrl,
                Roles = roles,
                CompanyId = companyId
            };

            return userInfo;
        }

        public static async Task<UserInfo?> GetUserInfoAsync(Task<AuthenticationState>? authStateTask)
        {
            if (authStateTask is null)
            {
                return null;
            }
            else
            {
                AuthenticationState authState = await authStateTask;
                UserInfo? userInfo = GetUserInfo(authState);
                return userInfo;
            }
        }
    }
}

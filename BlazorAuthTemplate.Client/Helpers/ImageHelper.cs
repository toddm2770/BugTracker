﻿using Microsoft.AspNetCore.Components.Forms;

namespace BlazorAuthTemplate.Client.Helpers
{
	public static class ImageHelper
	{
		public static readonly string DefaultProfilePicture = "/image/ProfileImage.jpg";
		public static readonly string DefaultCompanyImage = "/image/DemoCompanyImage.svg";
		public static readonly string DefaultProjectImage = "/image/DefaultProjectImage.png";
		public static int MaxFileSize = 5 * 1024 * 1024;

		public static async Task<string> GetDataUrl(IBrowserFile file)
		{
			using Stream fileStream = file.OpenReadStream(MaxFileSize);
			using MemoryStream ms = new MemoryStream();
			await fileStream.CopyToAsync(ms);

			byte[] imageBytes = ms.ToArray();
			string imageBase64 = Convert.ToBase64String(imageBytes);
			string dataUrl = $"data:{file.ContentType};base64,{imageBase64}";

			return dataUrl;
		}
    }
}

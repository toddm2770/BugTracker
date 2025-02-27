﻿using BlazorAuthTemplate.Client.Helpers;
using BlazorAuthTemplate.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;


namespace BlazorAuthTemplate.Helpers
{
    public static class UploadHelper
    {
        public static readonly string DefaultProfilePicture = ImageHelper.DefaultProfilePicture;
        public static readonly string DefaultCompanyImage = ImageHelper.DefaultCompanyImage;
        public static readonly string DefaultProjectImage = ImageHelper.DefaultProjectImage;
        public static int MaxFileSize = ImageHelper.MaxFileSize;

        public static async Task<FileUpload> GetImageUploadAsync(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            byte[] data = ms.ToArray();

            if (ms.Length > MaxFileSize)
            {
                throw new IOException("Images must be less than 5MB!");
            }

            FileUpload upload = new FileUpload()
            {
                Id = Guid.NewGuid(),
                Data = data
            };

            return upload;
        }

        public static FileUpload GetImageUpload(string dataUrl)
        {
            GroupCollection matchGroups = Regex.Match(dataUrl, @"data:(?<type>.+?);base64,(?<data>.+)").Groups;

            if (matchGroups.ContainsKey("type") && matchGroups.ContainsKey("data"))
            {
                string contentType = matchGroups["type"].Value;
                byte[] data = Convert.FromBase64String(matchGroups["data"].Value);

                if (data.Length <= 5 * 1024 * 1024)
                {
                    FileUpload upload = new FileUpload()
                    {
                        Id = Guid.NewGuid(),
                        Data = data,
                    };

                    return upload;
                }
            }

            throw new IOException("Data URL was either invalid or too large");
        }
    }

}

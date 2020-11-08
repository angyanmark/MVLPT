﻿using ImageStore.Backend.Common.Dtos.Image;
using ImageStore.Backend.Common.Exceptions;
using ImageStore.Backend.Common.Options;
using ImageStore.Backend.Dal;
using ImageStore.Backend.Dal.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImageStore.Backend.Bll.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly ImageOptions _imageOptions;
        private readonly ClaimsPrincipal _user;
        private readonly UserManager<User> _userManager;
        private readonly ImageStoreDbContext _dbContext;

        public ImageService(IOptions<ImageOptions> fileOptions, UserManager<User> userManager, ClaimsPrincipal user, ImageStoreDbContext dbContext)
        {
            _imageOptions = fileOptions.Value;
            _userManager = userManager;
            _user = user;
            _dbContext = dbContext;
        }

        public async Task<List<ImageDto>> GetImagesAsync()
        {
            return await _dbContext.Images.Select(image => new ImageDto
            {
                Id = image.Id,
                FileName = image.OriginalFileName,
                Url = $"/images/{image.FileName}",
                ThumbnailUrl = $"/thumbnails/{image.FileName}",
                Uploader = image.Uploader.UserName
            }).ToListAsync();
        }

        public async Task<ImageDto> UploadFileAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || !_imageOptions.AllowedFileExtensions.Contains(extension))
                throw new ImageStoreException($"'{extension}' extension is not allowed");

            var imageGuid = Guid.NewGuid();
            var fileName = imageGuid + ".png";
            var caffFileName = imageGuid + extension;
            var image = new Dal.Entities.Image
            {
                FileName = fileName,
                OriginalFileName = file.FileName,
                Uploader = await _userManager.GetUserAsync(_user)
            };

            _dbContext.Images.Add(image);

            var folderPath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath);
            var filePath = Path.Combine(folderPath, fileName);
            var caffFilePath = Path.Combine(folderPath, caffFileName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using (var stream = new FileStream(caffFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // TODO: thumbnail generálás

            var startInfo = new ProcessStartInfo("caff-parser.exe");
            var hexData = "";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = caffFilePath;
            var process = new Process {StartInfo = startInfo};
            var skip = 0;
            process.OutputDataReceived += (sender, args) =>
            {
                if(args.Data is null) return;
                if (args.Data.Length == 1)
                {
                    skip = int.Parse(args.Data);
                    return;
                }

                if (skip != 0)
                {
                    skip--;
                    return;
                }
                hexData += args.Data;
            };
            process.Start();
            process.BeginOutputReadLine();

            process.WaitForExit();

            var bytesCount = (hexData.Length) / 2;
            var bytes = new byte[bytesCount];
            for (var x = 0; x < bytesCount; ++x)
            {
                bytes[x] = Convert.ToByte(hexData.Substring(x * 2, 2), 16);
            }

            await File.WriteAllBytesAsync(filePath, bytes).ConfigureAwait(false);

            await _dbContext.SaveChangesAsync();

            return new ImageDto
            {
                Id = image.Id,
                FileName = image.OriginalFileName,
                Url = $"/images/{image.FileName}",
                ThumbnailUrl = $"/thumbnails/{image.FileName}",
                Uploader = image.Uploader.UserName
            };
        }

        public async Task DeleteFileAsync(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image is null)
                throw new ImageStoreException($"File with id '{id}' does not exist");

            _dbContext.Images.Remove(image);

            var filePath = Path.Combine(_imageOptions.RootPath, _imageOptions.FilesPath, image.FileName);

            await _dbContext.SaveChangesAsync();

            if (File.Exists(filePath)) 
                File.Delete(filePath);
        }

        public async Task<CommentDto> PostCommentAsync(int imageId, string text)
        {
            var image = await _dbContext.Images.FindAsync(imageId);
            if (image is null)
                throw new ImageStoreException($"File with id '{imageId}' does not exist");

            var comment = new Comment
            {
                Image = image,
                Text = text,
                Commenter = await _userManager.GetUserAsync(_user),
                Date = DateTime.UtcNow
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            return new CommentDto
            {
                Id = comment.Id,
                Commenter = comment.Commenter.UserName,
                Text = comment.Text,
                Date = comment.Date
            };
        }

        public async Task<List<CommentDto>> GetCommentsAsync(int imageId)
        {
            var image = await _dbContext.Images.FindAsync(imageId);
            if (image is null)
                throw new ImageStoreException($"File with id '{imageId}' does not exist");

            return await _dbContext.Comments
                .Where(comment => comment.Image.Id == imageId)
                .Select(comment => new CommentDto
                {
                    Id = comment.Id,
                    Commenter = comment.Commenter.UserName,
                    Text = comment.Text,
                    Date = DateTime.SpecifyKind(comment.Date, DateTimeKind.Utc)
                }).ToListAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment is null)
                throw new ImageStoreException($"Comment with id '{id}' does not exist");

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();
        }
    }
}

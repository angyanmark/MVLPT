using ImageStore.Backend.Bll.Services.Image;
using ImageStore.Backend.Common.Constants;
using ImageStore.Backend.Common.Dtos.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageStore.Backend.Web.Controllers
{
    [Authorize]
    public class ImageController : ApiControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<ImageDto> UploadImage(IFormFile file)
        {
            return await _imageService.UploadFileAsync(file);
        }

        [HttpGet]
        public async Task<List<ImageDto>> GetImages()
        {
            return await _imageService.GetImagesAsync();
        }

        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public async Task DeleteImage(int id)
        {
            await _imageService.DeleteFileAsync(id);
        }

        [HttpPost("comment")]
        public async Task<CommentDto> PostComment(int imageId, string text)
        {
            return await _imageService.PostCommentAsync(imageId, text);
        }

        [HttpGet("comment")]
        public async Task<List<CommentDto>> GetComments(int imageId)
        {
            return await _imageService.GetCommentsAsync(imageId);
        }

        [HttpDelete("comment")]
        [Authorize(Roles = Roles.Admin)]
        public async Task DeleteComment(int id)
        {
            await _imageService.DeleteCommentAsync(id);
        }
    }
}

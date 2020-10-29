using ImageStore.Backend.Common.Dtos.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageStore.Backend.Bll.Services.Image
{
    public interface IImageService
    {
        Task<List<ImageDto>> GetImagesAsync();
        Task<ImageDto> UploadFileAsync(IFormFile file);
        Task DeleteFileAsync(int id);
        Task<CommentDto> PostCommentAsync(int imageId, string text);
        Task<List<CommentDto>> GetCommentsAsync(int imageId);
        Task DeleteCommentAsync(int id);
    }
}

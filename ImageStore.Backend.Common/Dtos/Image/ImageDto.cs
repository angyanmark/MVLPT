using FluentValidation;
using System.Collections.Generic;

namespace ImageStore.Backend.Common.Dtos.Image
{
    public class ImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Uploader { get; set; }
    }
}

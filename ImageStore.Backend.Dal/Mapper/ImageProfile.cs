using AutoMapper;
using ImageStore.Backend.Common.Dtos.Account;
using ImageStore.Backend.Common.Dtos.Image;
using ImageStore.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Dal.Mapper
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDto>();
        }
    }
}

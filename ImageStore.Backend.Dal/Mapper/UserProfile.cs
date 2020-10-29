using AutoMapper;
using ImageStore.Backend.Common.Dtos.Account;
using ImageStore.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Dal.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<LoginDto, User>();
            CreateMap<RegisterDto, User>();
        }
    }
}

using AutoMapper;
using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile() {

            // Authentication
            CreateMap<User, AuthenAccountDTO>().ReverseMap();
            CreateMap<User, RegisterAccountDTO>().ReverseMap();
            // View, Get
            CreateMap<User, AccountDTO>().ReverseMap();

            //Create

            //Update
            CreateMap<User, AccountUpdateDTO>().ReverseMap();
            //Delete

            //Pagination
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}

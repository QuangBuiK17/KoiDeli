using AutoMapper;
using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile() 
        {
            CreateMap<Vehicle, VehicleDTO>().ReverseMap();
        }
    }
}

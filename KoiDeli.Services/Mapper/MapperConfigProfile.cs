using AutoMapper;
using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.BranchDTOs;
using KoiDeli.Domain.DTOs.KoiFishDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Domain.DTOs.VehicleDTOs;
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
            CreateMap<Box, BoxDTO>().ReverseMap();
            CreateMap<KoiFish, KoiFishDTO>().ReverseMap();
            CreateMap<PartnerShipment, PartnerShipmentDTO>().ReverseMap();
            CreateMap<Vehicle, VehicleDTO>().ReverseMap();
            CreateMap<Branch, BranchDTO>().ReverseMap();



            //Create
            CreateMap<Box, BoxCreateDTO>().ReverseMap();
            CreateMap<KoiFish, KoiFishCreateDTO>().ReverseMap();
            CreateMap<PartnerShipment, PartnerShipmentCreateDTO>().ReverseMap();
            CreateMap<Vehicle, VehicleCreateDTO>().ReverseMap();
            CreateMap<Branch, BranchCreateDTO>().ReverseMap();
            //Update
            CreateMap<User, AccountUpdateDTO>().ReverseMap();
            CreateMap<Box, BoxUpdateDTO>().ReverseMap();
            CreateMap<KoiFish, KoiFishUpdateDTO>().ReverseMap();
            CreateMap<Vehicle, VehicleUpdateDTO>().ReverseMap();
            CreateMap<Branch, BranchUpdateDTO>().ReverseMap();
            CreateMap<PartnerShipment, PartnerShipmentUpdateDTO>().ReverseMap();
            //Delete ko can

            //Pagination
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}

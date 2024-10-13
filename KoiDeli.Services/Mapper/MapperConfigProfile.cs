using AutoMapper;
using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Domain.DTOs.BoxDTOs;
using KoiDeli.Domain.DTOs.BoxOptionDTOs;
using KoiDeli.Domain.DTOs.BranchDTOs;
using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.FeedbackDTOs;
using KoiDeli.Domain.DTOs.KoiFishDTOs;
using KoiDeli.Domain.DTOs.OrderDetailDTOs;
using KoiDeli.Domain.DTOs.OrderDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Domain.DTOs.TimelineDeliveryDTOs;
using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.DTOs.UserDTOs;
using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
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
            CreateMap<Vehicle, VehicleDTO>().ReverseMap();
            CreateMap<Branch, BranchDTO>().ReverseMap();
            CreateMap<BoxOption, BoxOptionDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<OrderTimeline, OrderTimelineDTO>().ReverseMap();
            CreateMap<TimelineDelivery, TimelineDeliveryDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Distance, DistanceDTO>().ReverseMap();
            CreateMap<PartnerShipment, PartnerDTO>().ReverseMap();
            CreateMap<Wallet, WalletDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Feedback, FeedbackDTO>().ReverseMap();
            CreateMap<User, UserDetailsModel>().ReverseMap();
            CreateMap<Role, RoleInfoModel>().ReverseMap();
            CreateMap<User, RoleInfoModel>().ReverseMap();
            //Create
            CreateMap<Box, BoxCreateDTO>().ReverseMap();
            CreateMap<KoiFish, KoiFishCreateDTO>().ReverseMap();
            CreateMap<Vehicle, VehicleCreateDTO>().ReverseMap();
            CreateMap<Branch, BranchCreateDTO>().ReverseMap();
            CreateMap<BoxOption, BoxOptionCreateDTO>().ReverseMap();
            CreateMap<Order, OrderCreateDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailCreateDTO>().ReverseMap();
            CreateMap<OrderTimeline, OrderTimelineCreateDTO>().ReverseMap();
            CreateMap<TimelineDelivery, TimelineDeliveryCreateDTO>().ReverseMap();
            CreateMap<Role, RoleCreateDTO>().ReverseMap();
            CreateMap<Distance, DistanceCreateDTO>().ReverseMap();
            CreateMap<PartnerShipment, PartnerCreateDTO>().ReverseMap();
            CreateMap<Wallet, WalletCreateDTO>().ReverseMap();
            CreateMap<Transaction, TransactionCreateDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<Feedback, FeedbackCreateDTO>().ReverseMap();

            //Update
            CreateMap<User, AccountUpdateDTO>().ReverseMap();
            CreateMap<Box, BoxUpdateDTO>().ReverseMap();
            CreateMap<KoiFish, KoiFishUpdateDTO>().ReverseMap();
            CreateMap<Vehicle, VehicleUpdateDTO>().ReverseMap();
            CreateMap<Branch, BranchUpdateDTO>().ReverseMap();
            CreateMap<BoxOption, BoxOptionUpdateDTO>().ReverseMap();
            CreateMap<Order, OrderUpdateDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailUpdateDTO>().ReverseMap();
            CreateMap<OrderTimeline, OrderTimelineUpdateDTO>().ReverseMap();
            CreateMap<TimelineDelivery, TimelineDeliveryUpdateDTO>().ReverseMap();
            CreateMap<Role, RoleUpdateDTO>().ReverseMap();
            CreateMap<Distance, DistanceUpdateDTO>().ReverseMap();
            CreateMap<PartnerShipment, PartnerUpdateDTO>().ReverseMap();
            CreateMap<PartnerShipment, PartnerUpdateCompleteDTO>().ReverseMap();
            CreateMap<Wallet, WalletUpdateDTO>().ReverseMap();
            CreateMap<Transaction, TransactionUpdateDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<Feedback, FeedbackUpdateDTO>().ReverseMap();
            //Delete ko can

            //Pagination
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
        }
    }
}

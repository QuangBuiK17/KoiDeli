using AutoMapper;
using Azure;
using KoiDeli.Domain.DTOs.BranchDTOs;
using KoiDeli.Domain.DTOs.DeliveryDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Domain.DTOs.TimelineDeliveryDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Domain.Enums;
using KoiDeli.Repositories;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Repositories.Repositories;
using KoiDeli.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly KoiDeliDbContext _context;
        private readonly IMapper _mapper;
        private readonly IOrderTimelineService _orderTimelineService;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public DeliveryService(KoiDeliDbContext context,
                                IMapper mapper,
                                IOrderTimelineService orderTimelineService,
                                ICurrentTime currentTime,
                                AppConfiguration appConfiguration,
                                IUnitOfWork unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _orderTimelineService = orderTimelineService;
            _currentTime = currentTime;
            _configuration = appConfiguration;
            _unitOfWork = unitOfWork;
        }

        public Task<ApiResult<bool>> AssignOrderToTimelineAsync(int timelineDeliveryID, int orderDetailID)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<de_OrderDetailInfoDTO>> GetOrderDetailInfoAsync(int orderDetailID)
        {
            var result = new ApiResult<de_OrderDetailInfoDTO>();

            try
            {
                var orderDetail = await _context.OrderDetails
                    .Where(o => o.Id == orderDetailID && o.IsDeleted == false)
                    .Join(_context.BoxOptions, o => o.BoxOptionId, b => b.Id, (o, b) => new { o, b })
                    .Join(_context.Boxes, ob => ob.b.BoxId, bx => bx.Id, (ob, bx) => new de_OrderDetailInfoDTO
                    {
                        OrderDetailID = ob.o.Id,
                        BoxOptionID = ob.o.BoxOptionId,
                        BoxName = bx.Name,
                        BoxVolume = bx.MaxVolume,
                    })
                    .FirstOrDefaultAsync();

                if (orderDetail == null)
                {
                    result.Success = false;
                    result.Message = "Order detail not found or has been deleted.";
                }
                else
                {
                    result.Success = true;
                    result.Data = orderDetail;
                    result.Message = $"Get orderDetail with ID( {orderDetailID} ) sucessfully";
                }
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.ErrorMessages = new List<string> { ex.Message };
            }

            return result;
        }



          public async Task<ApiResult<List<de_TimelineDeliveryInfoDTO>>> GetTimelineDeliveriesAsync(int branchID, DateTime date)
          {
              var result = new ApiResult<List<de_TimelineDeliveryInfoDTO>>();

              try
              {
                  var deliveries = await _context.TimelineDelivery
                      .Where(t => t.BranchId == branchID && t.StartDay.Date == date.Date)
                      .Join(_context.Branches, t => t.BranchId, b => b.Id, (t, b) => new { t, b })
                      .Join(_context.Vehicles, tb => tb.t.VehicleId, v => v.Id, (tb, v) => new de_TimelineDeliveryInfoDTO
                      {
                          TimelineDeliveryID = tb.t.Id,
                          BranchID = tb.t.BranchId,
                          StartPoint = tb.b.StartPoint,
                          EndPoint = tb.b.EndPoint,
                          StartDay = tb.t.StartDay.ToString("yyyy-MM-dd"),
                          EndDay = tb.t.EndDay.ToString("yyyy-MM-dd"),
                          StartTime = tb.t.StartDay.ToString("HH-mm-ss"),
                          EndTime = tb.t.EndDay.ToString("HH-mm-ss"),
                          VehicleID = tb.t.VehicleId,
                          VehicleName = v.Name,
                          MaxVolume = v.VehicleVolume,
                          RemainingVolume = v.VehicleVolume - _context.OrderTimeline
                          .Where(ot => ot.TimelineDeliveryId == tb.t.Id && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false)
                          .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume)
                      })
                      .ToListAsync();
                  if (deliveries != null)
                  {
                      result.Success = true;
                      result.Data = deliveries;
                      result.Message = $"There are {deliveries.Count} trip in date: {date.ToString("yyyy-MM-dd")}.";
                  }else
                  {
                      result.Success = false;
                      result.Message = "There are no matching trip.";
                  }

              }
              catch (Exception ex)
              {
                  result.Success = false;
                  result.Message = ex.Message;
              }

              return result;
          }

         public async Task<ApiResult<OrderTimelineDTO>> CreateOrderTimelineAsync(OrderTimelineCreateDTO orderTimelineDto)
         {
             var response = new ApiResult<OrderTimelineDTO>();

             try
             {
                 var orderTimeline = await _context.OrderTimeline.FirstOrDefaultAsync(ot => ot.OrderDetailId == orderTimelineDto.OrderDetailId
                                                                                     && ot.TimelineDeliveryId == orderTimelineDto.TimelineDeliveryId);
                
                 if (orderTimeline != null)
                 {
                     response.Success = false;
                     response.Message = "This orderDetail has been assigned into this timeline.";
                     return response;
                 }
                 
                 //var timeline = await _context.TimelineDelivery.FindAsync(orderTimelineDto.TimelineDeliveryId);
                 //var orderDetail = await _context.OrderDetails.FindAsync(orderTimelineDto.OrderDetailId);

                 var timeline = await _context.TimelineDelivery
                     .Include(t => t.Vehicle)
                     .FirstOrDefaultAsync(t => t.Id == orderTimelineDto.TimelineDeliveryId);

                 // Lấy thông tin OrderDetail cùng với BoxOption và Box
                 var orderDetail = await _context.OrderDetails
                     .Include(od => od.BoxOption)
                     .ThenInclude(bo => bo.Box)
                     .FirstOrDefaultAsync(od => od.Id == orderTimelineDto.OrderDetailId);

                 if (timeline == null || orderDetail == null)
                 {
                     response.Success = false;
                     response.Message = "Timeline or Order detail not found.";
                     return response;
                 }

                 if (timeline.Vehicle == null)
                 {
                     response.Success = false;
                     response.Message = "Vehicle not found for the timeline.";
                     return response;
                 }

                 if (orderDetail.BoxOption == null || orderDetail.BoxOption.Box == null)
                 {
                     response.Success = false;
                     response.Message = "Invalid Box or BoxOption data.";
                     return response;
                 }

                 var remainingVolume = timeline.Vehicle.VehicleVolume -
                     _context.OrderTimeline
                         .Where(ot => ot.TimelineDeliveryId == orderTimelineDto.TimelineDeliveryId 
                                     && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false)
                         .Sum(ot =>  ot.OrderDetail.BoxOption.Box.MaxVolume);

                 var orderTotalVolume =  orderDetail.BoxOption.Box.MaxVolume;

                 if (orderTotalVolume > remainingVolume)
                 {
                     response.Success = false;
                     //response.Message = "Not enough space in the timeline delivery.";
                     response.Message = $"Can not assign orderDetail_ID: {orderTimelineDto.OrderDetailId} with total volume = {orderTotalVolume} " +
                                        $"into the timeline delivery: {orderTimelineDto.TimelineDeliveryId}, because the remainingVolume = {remainingVolume}";
                 }
                 else
                 {
                     var entity = _mapper.Map<OrderTimeline>(orderTimelineDto);

                     entity.IsCompleted = orderTimelineDto.IsCompleted.HasValue
                         ? orderTimelineDto.IsCompleted.Value.ToString()
                         : StatusEnum.Pending.ToString();

                     await _unitOfWork.OrderTimelineRepository.AddAsync(entity);

                     if (await _unitOfWork.SaveChangeAsync() > 0)
                     {
                         response.Data = _mapper.Map<OrderTimelineDTO>(entity);
                         response.Success = true;
                         response.Message = $"OrderTimeline created successfully. " +
                                             $"orderDetail_ID: {orderTimelineDto.OrderDetailId} with total volume = {orderTotalVolume} " +
                                             $"has been assigned into the timeline delivery: {orderTimelineDto.TimelineDeliveryId} ";
                     }
                     else
                     {
                         response.Success = false;
                         response.Message = "Failed to create OrderTimeline.";
                     }
                 }
             }
             catch (Exception ex)
             {
                 response.Success = false;
                 response.ErrorMessages = new List<string> { ex.Message };
             }

             return response;
         }
        
        public async Task<ApiResult<bool>> CreateTotalTimelineAsync (de_CreateTotalTimelineDTO dto)
        {
            var response = new ApiResult<bool>();
            var timelines = new List<TimelineDelivery>();
            var totalDescription = "";
            try
            {

                //CHECK VehicleID
                var existVehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(dto.VehicleID);
                if (existVehicle == null)
                {
                    response.Success = false;
                    response.Message = $"Invalid VehicleID: {dto.VehicleID}";
                    return response;
                }

                //CHECK DOULICATE BranchID

                var duplicateBranchIds = dto.de_CreateDetailTimelineDTOs
                    .GroupBy(t => t.BranchId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateBranchIds.Any())
                {
                    response.Success = false;
                    response.Message = $"Duplicate BranchID(s) found: {string.Join(", ", duplicateBranchIds)}";
                    return response;
                }


                //CHECK StartDay < EndDay
                var invalidTimelines = dto.de_CreateDetailTimelineDTOs
                    .Where(t => t.StartDay >= t.EndDay)
                    .ToList();

                if (invalidTimelines.Any())
                {
                    response.Success = false;
                    response.Message = "Invalid timelines detected: StartDay must be earlier than EndDay.";
                    return response;
                }


                //CHECK DATE in DTO
                var isConflictWithinDTO = dto.de_CreateDetailTimelineDTOs
            .       Any(t1 => dto.de_CreateDetailTimelineDTOs
                    .Any(t2 => t1 != t2 &&
                            //t1.BranchId == t2.BranchId &&  // Cùng Branch
                            t1.StartDay <= t2.EndDay && t1.EndDay >= t2.StartDay)); // Xung đột thời gian

                if (isConflictWithinDTO)
                {
                    response.Success = false;
                    response.Message = "Conflicting timelines detected within the request.";
                    return response;
                }


                //SET DESCRIPTION.
                totalDescription = "";
                if (dto.de_CreateDetailTimelineDTOs.Count > 1)
                {
                    totalDescription = $"This timeline belong to bigger one, with total {dto.de_CreateDetailTimelineDTOs.Count} timelines.";
                }

                //Creta some variable before enter the loop
                var existingTimelines = await _unitOfWork.TimelineDeliveryRepository.GetAllAsync();
                int count = 1;

                foreach (var detailTimeline in dto.de_CreateDetailTimelineDTOs)
                {
                    
                    //CHECK branchID
                    var existBranch = await _unitOfWork.BranchRepository.GetByIdAsync(detailTimeline.BranchId);
                    if (existBranch == null)
                    {
                        response.Success = false;
                        response.Message = $"Invalid branchID: {detailTimeline.BranchId}";
                        return response;
                    }

                    //CHECK LOGIC DAY

                    var isConflictWithExisting = existingTimelines.Any(t =>
                            t.VehicleId == dto.VehicleID &&
                            t.BranchId == detailTimeline.BranchId &&
                            (detailTimeline.StartDay <= t.EndDay && detailTimeline.EndDay >= t.StartDay));

                    if (isConflictWithExisting)
                    {
                        response.Success = false;
                        response.Message = "The vehicle is already scheduled for another delivery within the given time range.";
                        return response;
                    }

                    totalDescription += $"\n{count}) Vehicle: {existVehicle.Name}, Branch: {existBranch.Name}, " +
                                $"Start: {detailTimeline.StartDay}, End: {detailTimeline.EndDay}";


                    var timeline = new TimelineDelivery
                    {
                        VehicleId = dto.VehicleID,
                        BranchId = detailTimeline.BranchId,
                        StartDay = detailTimeline.StartDay,
                        EndDay = detailTimeline.EndDay,
                        IsCompleted = StatusEnum.Pending.ToString(),
                        TimeCompleted = null,
                       
                    };
                    timelines.Add(timeline);
                    count++;
                }


                foreach (var timeline in timelines)
                {
                    timeline.Description = totalDescription;
                }

                await _unitOfWork.TimelineDeliveryRepository.AddRangeAsync(timelines);


                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Success = true;
                    response.Data = true;
                    response.Message = "Timelines created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create Timelines.";
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
    }
}

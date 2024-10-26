using AutoMapper;
using Azure;
using KoiDeli.Domain.DTOs.BranchDTOs;
using KoiDeli.Domain.DTOs.DeliveryDTOs;
using KoiDeli.Domain.DTOs.OrderDetailDTOs;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                        isComplete = ob.o.IsComplete,
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
                          isComplete = tb.t.IsCompleted,
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
                          CurentVolume = _context.OrderTimeline
                                            .Where(ot => ot.TimelineDeliveryId == tb.t.Id && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                                                                          && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                                          && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                                            .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume),

                          /*RemainingVolume = v.VehicleVolume - _context.OrderTimeline
                          .Where(ot => ot.TimelineDeliveryId == tb.t.Id && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false  
                                                                        && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                        && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                          .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume)*/
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
                                     && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                     && ot.IsCompleted != StatusEnum.Completed.ToString()
                                     && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
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


        public async Task<ApiResult<bool>> CreateTotalTimelineAsync(de_CreateTotalTimelineDTO dto)
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




                //SET DESCRIPTION.
                totalDescription = "";
                if (dto.de_CreateDetailTimelineDTOs.Count > 1)
                {
                    totalDescription = $"This timeline belong to bigger one, with total {dto.de_CreateDetailTimelineDTOs.Count} timelines.";
                }

                //Creta some variable before enter the loop
                //var existingTimelines = await _unitOfWork.TimelineDeliveryRepository.GetAllAsync();
                var existingTimelines = await _unitOfWork.TimelineDeliveryRepository.SearchAsync(o => o.IsDeleted == false);
                DateTime currentStartDay = dto.TotalStartTime;
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


                    // SET start day and end day
                    TimeSpan duration = SetDuarationTimeByBranch(detailTimeline.BranchId);
                    DateTime endDay = currentStartDay + duration;

                    //CHECK LOGIC DAY

                    var isConflictWithExisting = existingTimelines.Any(t =>
                            t.VehicleId == dto.VehicleID &&
                            t.BranchId == detailTimeline.BranchId &&
                            (currentStartDay <= t.EndDay && endDay >= t.StartDay));

                    if (isConflictWithExisting)
                    {
                        response.Success = false;
                        response.Message = "The vehicle is already scheduled for another delivery within the given time range.";
                        return response;
                    }

                    totalDescription += $"\n{count}) Vehicle: {existVehicle.Name}, Branch: {existBranch.Name}, " +
                        $"Start: {currentStartDay}, End: {endDay}";


                    var timeline = new TimelineDelivery
                    {
                        VehicleId = dto.VehicleID,
                        BranchId = detailTimeline.BranchId,
                        StartDay = currentStartDay,
                        EndDay = endDay,
                        IsCompleted = StatusEnum.Pending.ToString(),
                        TimeCompleted = null,

                    };
                    timelines.Add(timeline);
                    count++;

                    currentStartDay = endDay;
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

        public async Task<ApiResult<bool>> AssignOrderToTimelineAsync(de_AssignOrderToTimelinesDTO dto)
        {
            var response = new ApiResult<bool>();
            var orderTimelines = new List<OrderTimeline>();

            try
            {
                // Kiểm tra OrderDetailID có tồn tại không
                var orderDetail = await _context.OrderDetails
                    .Include(od => od.BoxOption)
                    .ThenInclude(bo => bo.Box)
                    .FirstOrDefaultAsync(od => od.Id == dto.OrderDetailID);

                if (orderDetail == null || orderDetail.BoxOption?.Box == null)
                {
                    response.Success = false;
                    response.Message = "Invalid OrderDetail.";
                    return response;
                }

                var orderTotalVolume = orderDetail.BoxOption.Box.MaxVolume;

                // Duyệt qua từng TimelineID trong danh sách
                foreach (var timelineID in dto.TimelineID)
                {
                    // Kiểm tra TimelineDelivery có tồn tại không
                    var timeline = await _context.TimelineDelivery
                        .Include(t => t.Vehicle)
                        .FirstOrDefaultAsync(t => t.Id == timelineID);

                    if (timeline == null || timeline.Vehicle == null)
                    {
                        response.Success = false;
                        response.Message = $"Timeline {timelineID} invalid.";
                        return response;
                    }

                    // Kiểm tra xem OrderDetail đã được gán vào Timeline này chưa
                    var existingOrderTimeline = await _context.OrderTimeline
                        .FirstOrDefaultAsync(ot => ot.OrderDetailId == dto.OrderDetailID && ot.TimelineDeliveryId == timelineID
                                                    && ot.IsDeleted == false  && ot.IsCompleted != StatusEnum.Completed.ToString());

                    if (existingOrderTimeline != null)
                    {
                        response.Success = false;
                        response.Message = $"OrderDetail {dto.OrderDetailID} has assigned into Timeline {timelineID}.";
                        return response;
                    }

                    // Kiểm tra còn đủ không gian không
                    var remainingVolume = timeline.Vehicle.VehicleVolume -
                        _context.OrderTimeline
                            .Where(ot => ot.TimelineDeliveryId == timelineID 
                                        && !ot.IsDeleted && !ot.OrderDetail.IsDeleted
                                        && ot.IsCompleted != StatusEnum.Completed.ToString()
                                        && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                            .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume);

                    if (orderTotalVolume > remainingVolume)
                    {
                        response.Success = false;
                        //response.Message = $"Không đủ không gian trong Timeline {timelineID}. Dung tích còn lại: {remainingVolume}.";
                        response.Message = $"This OrderDetail can not assign into timeline {timelineID}, because remaning volume = {remainingVolume}.";
                        return response;
                    }

                    // Tạo mới OrderTimeline
                    var newOrderTimeline = new OrderTimeline
                    {
                        OrderDetailId = dto.OrderDetailID,
                        TimelineDeliveryId = timelineID,
                        StartDay = timeline.StartDay,
                        EndDay = timeline.EndDay,
                        IsCompleted = StatusEnum.Pending.ToString(),
                        TimeCompleted = null,
                        Description = null
                    };

                    orderTimelines.Add(newOrderTimeline);
                }

                // Thêm danh sách OrderTimeline vào database
                await _unitOfWork.OrderTimelineRepository.AddRangeAsync(orderTimelines);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Success = true;
                    response.Data = true;
                    response.Message = $"Total {orderTimelines.Count} OrderTimelines has been assign successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Can not create OrderTimelines.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }



        public async Task<ApiResult<bool>> UpdateTimelineStatusAsync(int timelineID)
        {
            var response = new ApiResult<bool>();

            try
            {
                var timeline = await _context.TimelineDelivery.FirstOrDefaultAsync(t => t.Id == timelineID);

                if (timeline == null)
                {
                    response.Success = false;
                    response.Message = $"Timeline ID: {timelineID} invalid.";
                    return response;
                }


                if(timeline.IsCompleted == StatusEnum.Pending.ToString())
                {
                    timeline.IsCompleted = StatusEnum.Delivering.ToString();
                }else if (timeline.IsCompleted == StatusEnum.Delivering.ToString())
                {
                    timeline.IsCompleted = StatusEnum.Completed.ToString();
                    timeline.TimeCompleted = _currentTime.GetCurrentTime();
                }else if (timeline.IsCompleted == StatusEnum.Completed.ToString())
                {
                    response.Success = false;
                    response.Message = $"Timeline ID: {timelineID} has been completed.";
                    return response;
                }          
                _unitOfWork.TimelineDeliveryRepository.Update(timeline);

                // get all ordertimeline in this timeline
                var relatedOrderTimelines = await _context.OrderTimeline
                    .Where(ot => ot.TimelineDeliveryId == timelineID && !ot.IsDeleted
                                    && ot.IsCompleted != StatusEnum.Completed.ToString())
                    .ToListAsync();

                if (relatedOrderTimelines.Any())
                {
                    // Cập nhật tất cả OrderTimeline thành Completed
                    foreach (var orderTimeline in relatedOrderTimelines)
                    {
                        if (orderTimeline.IsCompleted == StatusEnum.Pending.ToString())
                        {
                            orderTimeline.IsCompleted = StatusEnum.Delivering.ToString();
                            _unitOfWork.OrderTimelineRepository.Update(orderTimeline);
                        }
                        else if (orderTimeline.IsCompleted == StatusEnum.Delivering.ToString())
                        {
                            orderTimeline.IsCompleted = StatusEnum.Completed.ToString();
                            orderTimeline.TimeCompleted = _currentTime.GetCurrentTime();
                            _unitOfWork.OrderTimelineRepository.Update(orderTimeline);
                        }
                    }
                }


                var orderDetailsToCheck = await _context.OrderTimeline
                                        .Where(ot => ot.TimelineDeliveryId == timelineID && !ot.IsDeleted)
                                        .Select(ot => ot.OrderDetailId)
                                        .Distinct()
                                        .ToListAsync();
                foreach (var orderDetailId in orderDetailsToCheck)
                {
                    // Lấy tất cả các OrderTimeline của OrderDetail đó
                    var allOrderTimelines = await _context.OrderTimeline
                        .Where(ot => ot.OrderDetailId == orderDetailId && !ot.IsDeleted)
                        .ToListAsync();

                    var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.Id == orderDetailId);

                    // Kiểm tra xem tất cả các OrderTimeline có trạng thái Completed hay không
                    if (allOrderTimelines.All(ot => ot.IsCompleted == StatusEnum.Completed.ToString()))
                    {
                        // Nếu tất cả OrderTimeline đều đã Completed, cập nhật trạng thái OrderDetail
                        //var orderDetail = await _context.OrderDetails.FirstOrDefaultAsync(od => od.Id == orderDetailId);
                        if (orderDetail != null && orderDetail.IsComplete == StatusEnum.Delivering.ToString())
                        {
                            orderDetail.IsComplete = StatusEnum.Completed.ToString();
                            _unitOfWork.OrderDetailRepository.Update(orderDetail);
                        }
                    }
                    else
                    {
                        if (orderDetail != null && orderDetail.IsComplete == StatusEnum.Packed.ToString())
                        {
                            orderDetail.IsComplete = StatusEnum.Delivering.ToString();
                            _unitOfWork.OrderDetailRepository.Update(orderDetail);
                        }
                    }
                }



                // Lưu thay đổi vào database
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Success = true;
                    response.Data = true;
                    response.Message = $"Total {relatedOrderTimelines.Count} ordertimeline and timeline ({timelineID}) has been updated to {timeline.IsCompleted}.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Update status fail.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }



        public async Task<ApiResult<de_ViewScheduleOfOrdetailDTO>> ViewScheduleOfOrdetail(int orderDetailID)
        {
            var response = new ApiResult<de_ViewScheduleOfOrdetailDTO>();

            try
            {
                // Tìm OrderDetail với các thông tin liên quan đến BoxOption và Box.
                var orderDetail = await _context.OrderDetails
                    .Include(od => od.BoxOption)
                    .ThenInclude(bo => bo.Box)
                    .FirstOrDefaultAsync(od => od.Id == orderDetailID);

                if (orderDetail == null)
                {
                    response.Success = false;
                    response.Message = $"OrderDetail with ID {orderDetailID} not found.";
                    return response;
                }

                // Lấy danh sách các OrderTimeline liên quan đến OrderDetail.
                var orderTimelines = await _context.OrderTimeline
                    .Where(ot => ot.OrderDetailId == orderDetailID)
                    .Include(ot => ot.TimelineDelivery)
                    .Select(ot => new de_OrderTimelineDTO
                    {
                        OrderTimelineID = ot.Id,
                        IsComplete = ot.IsCompleted,
                        TimelineID = ot.TimelineDeliveryId,
                        StartDay = ot.StartDay,
                        EndDay = ot.EndDay,
                        Start_From = ot.TimelineDelivery.Branch.Name
                    })
                    .ToListAsync();

                // Tạo DTO chứa dữ liệu trả về.
                var result = new de_ViewScheduleOfOrdetailDTO
                {
                    DetailID = orderDetail.Id,
                    IsComplete = orderDetail.IsComplete,
                    BoxOptionID = orderDetail.BoxOption.Id,
                    BoxName = orderDetail.BoxOption.Box.Name,
                    Volume = orderDetail.BoxOption.Box.MaxVolume,
                    OrderTimelines = orderTimelines
                };

                response.Success = true;
                response.Data = result;
                response.Message = $"Order detail and {orderTimelines.Count} related timelines retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }


        public async Task<ApiResult<de_ViewOrderDetailInTimelineDTO>> ViewAllOrderDetailInTimeline(int timelineID)
        {
            var response = new ApiResult<de_ViewOrderDetailInTimelineDTO>();

            try
            {
                // Lấy thông tin timeline cùng với Vehicle và Branch.
                var timeline = await _context.TimelineDelivery
                    .Include(t => t.Vehicle)
                    .Include(t => t.Branch)
                    .FirstOrDefaultAsync(t => t.Id == timelineID);

                if (timeline == null)
                {
                    response.Success = false;
                    response.Message = $"Timeline with ID {timelineID} not found.";
                    return response;
                }

                // Lấy danh sách OrderDetail liên quan đến timeline này.
                var orderDetails = await _context.OrderTimeline
                    .Where(ot => ot.TimelineDeliveryId == timelineID && ot.IsDeleted == false)
                    .Include(ot => ot.OrderDetail)
                        .ThenInclude(od => od.BoxOption)
                        .ThenInclude(bo => bo.Box)
                    .Select(ot => new de_OrderDetailDTO
                    {
                        DetailID = ot.OrderDetailId,
                        IsComplete = ot.OrderDetail.IsComplete,
                        BoxName = ot.OrderDetail.BoxOption.Box.Name,
                        Volume = ot.OrderDetail.BoxOption.Box.MaxVolume
                    })
                    .ToListAsync();

                // Tạo DTO chứa kết quả trả về.
                var result = new de_ViewOrderDetailInTimelineDTO
                {
                    TimelineID = timeline.Id,
                    IsComplete = timeline.IsCompleted,
                    VehicleName = timeline.Vehicle.Name,
                    BranchName = timeline.Branch.Name,
                    StartDay = timeline.StartDay,
                    EndDay = timeline.EndDay,
                    Maxvolume = timeline.Vehicle.VehicleVolume,
                    CurrentVolume = _context.OrderTimeline
                          .Where(ot => ot.TimelineDeliveryId == timelineID && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                                                            && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                            && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                          .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume),

                    /*RemainingVolume = timeline.Vehicle.VehicleVolume - _context.OrderTimeline
                          .Where(ot => ot.TimelineDeliveryId == timelineID  && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                                                            && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                            && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                          .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume),*/

                    OrderDetails = orderDetails
                };

                response.Success = true;
                response.Data = result;
                response.Message = $"Timeline and {orderDetails.Count} related order details retrieved successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<de_SuitableTimelineDTO>>> GetSuitableTimelines(int orderDetailID, int startBranch, int endBranch, DateTime? startDay)
        {
            var response = new ApiResult<List<de_SuitableTimelineDTO>>();
            try
            {
                var orderDetail = await _context.OrderDetails
                      .Include(od => od.BoxOption)
                      .ThenInclude(bo => bo.Box)
                      .FirstOrDefaultAsync(od => od.Id == orderDetailID && !od.IsDeleted);
                if (orderDetail == null)
                {
                    response.Success = false;
                    response.Message = $"OrderDetail ID: {orderDetailID} not found.";
                    return response;
                }

                var orderDetailVolume = orderDetail.BoxOption?.Box?.MaxVolume ?? 0;

                if (orderDetailVolume == 0)
                {
                    response.Success = false;
                    response.Message = "OrderDetail does not have a valid Box or BoxOption with volume.";
                    return response;
                }


                // Lấy các timeline phù hợp với các điều kiện: startBranch, endBranch, startDay và remainingVolume
                var suitableTimelines = await _context.TimelineDelivery
                    .Include(t => t.Vehicle)
                    .Where(t => t.BranchId >= startBranch && t.BranchId <= endBranch
                                && t.StartDay.Date == startDay
                                && !t.IsDeleted
                                && t.Vehicle.VehicleVolume -
                                    _context.OrderTimeline
                                        .Where(ot => ot.TimelineDeliveryId == t.Id && !ot.IsDeleted
                                                    && ot.OrderDetail.IsDeleted == false
                                                    && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                    && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                                        .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume)
                                        > orderDetailVolume)

                    .Select(t => new ListTimeline
                    {
                        TimelineId = t.Id,
                        isComplete = t.IsCompleted,
                        BranchName = t.Branch.Name,  // Assuming StartBranch has a 'Name' field
                        StartDate = t.StartDay,
                        EndDate = t.EndDay,
                        CurrentVolume = _context.OrderTimeline
                                            .Where(ot => ot.TimelineDeliveryId == t.Id && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                                                                       && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                                       && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                                            .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume),
                        RemainingVolume = t.Vehicle.VehicleVolume -
                                                _context.OrderTimeline
                                            .Where(ot => ot.TimelineDeliveryId == t.Id && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                                                                       && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                                       && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                                            .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume),

                    })
                    .ToListAsync();

                if (!suitableTimelines.Any())
                {
                    response.Success = false;
                    response.Message = "No suitable timelines found.";
                    return response;
                }

                var groupedTimelines = await _context.TimelineDelivery
                                         .Where(t => suitableTimelines.Select(st => st.TimelineId).Contains(t.Id)) // Chỉ lấy những timeline phù hợp
                                         .GroupBy(t => t.VehicleId)
                                         .Select(g => new de_SuitableTimelineDTO
                                         {
                                             VehicleID = g.Key,
                                             VehicleName = g.FirstOrDefault().Vehicle.Name, // Lấy tên xe từ bản ghi đầu tiên
                                             VehicleVolume = g.FirstOrDefault().Vehicle.VehicleVolume, // Lấy dung tích xe từ bản ghi đầu tiên
                                             Timelines = g.Select(t => new ListTimeline
                                             {
                                                 TimelineId = t.Id,
                                                 isComplete = t.IsCompleted,
                                                 BranchName = t.Branch.Name,  // Lấy tên chi nhánh bắt đầu
                                                 StartDate = t.StartDay,
                                                 EndDate = t.EndDay,
                                                 CurrentVolume = _context.OrderTimeline
                                                                .Where(ot => ot.TimelineDeliveryId == t.Id && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                                                                                           && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                                                           && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                                                                .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume),
                                                 RemainingVolume = t.Vehicle.VehicleVolume -
                                                                         _context.OrderTimeline
                                                                     .Where(ot => ot.TimelineDeliveryId == t.Id && ot.IsDeleted == false && ot.OrderDetail.IsDeleted == false
                                                                                                                && ot.IsCompleted != StatusEnum.Completed.ToString()
                                                                                                                && ot.OrderDetail.IsComplete != StatusEnum.Completed.ToString())
                                                                     .Sum(ot => ot.OrderDetail.BoxOption.Box.MaxVolume),
                                             }).ToList()
                                          }).ToListAsync();

                response.Data = groupedTimelines;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }







































        private TimeSpan SetDuarationTimeByBranch(int branchID)
        {
            return branchID switch
            {
                // Cần Thơ - Sài Gòn hoặc Sài Gòn - Cần Thơ
                1 or 2 => TimeSpan.FromHours(4),

                // Sài Gòn - Đà Nẵng hoặc Đà Nẵng - Sài Gòn
                3 or 4 => TimeSpan.FromHours(17),

                // Đà Nẵng - Hải Phòng hoặc Hải Phòng - Đà Nẵng
                5 or 6 => TimeSpan.FromHours(16),

                // Hải Phòng - Hà Nội hoặc Hà Nội - Hải Phòng
                7 or 8 => TimeSpan.FromHours(5),

                // Mặc định nếu branchID không hợp lệ
                _ => throw new ArgumentException("Invalid branchID")
            };
        }


        //Create multiple timeline but have to include multiple time


        /*
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
        }*/


    }
}

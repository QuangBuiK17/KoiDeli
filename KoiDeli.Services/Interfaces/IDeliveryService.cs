﻿using KoiDeli.Domain.DTOs.BranchDTOs;
using KoiDeli.Domain.DTOs.DeliveryDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IDeliveryService
    {
        Task<ApiResult<de_OrderDetailInfoDTO>> GetOrderDetailInfoAsync(int orderDetailID);
        Task<ApiResult<List<de_TimelineDeliveryInfoDTO>>> GetTimelineDeliveriesAsync(int branchID, DateTime date);
        Task<ApiResult<bool>> AssignOrderToTimelineAsync(int timelineDeliveryID, int orderDetailID);
        Task<ApiResult<OrderTimelineDTO>> CreateOrderTimelineAsync(OrderTimelineCreateDTO orderTimeline);

    }
}

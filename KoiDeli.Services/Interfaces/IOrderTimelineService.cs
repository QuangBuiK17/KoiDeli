using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IOrderTimelineService
    {
        Task<ApiResult<List<OrderTimelineDTO>>> GetOrderTimelinesAsync();
        Task<ApiResult<OrderTimelineDTO>> GetOrderTimelineByIdAsync(int id);
        Task<ApiResult<List<OrderTimelineDTO>>> SearchOrderTimelineByNameAsync(string name);
        Task<ApiResult<OrderTimelineDTO>> DeleteOrderTimelineAsync(int id);
        Task<ApiResult<OrderTimelineDTO>> UpdateOrderTimelineAsync(int id, OrderTimelineUpdateDTO updateDto);
        Task<ApiResult<OrderTimelineDTO>> CreateOrderTimelineAsync(OrderTimelineCreateDTO orderTimeline);

    }
}

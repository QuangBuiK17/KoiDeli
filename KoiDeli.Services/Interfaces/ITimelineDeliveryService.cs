using KoiDeli.Domain.DTOs.TimelineDeliveryDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface ITimelineDeliveryService
    {
        Task<ApiResult<List<TimelineDeliveryDTO>>> GetTimelineDeliveriesAsync();
        Task<ApiResult<List<TimelineDeliveryDTO>>> GetTimelineDeliveriesEnableAsync();
        Task<ApiResult<TimelineDeliveryDTO>> GetTimelineDeliveryByIdAsync(int id);
        Task<ApiResult<List<TimelineDeliveryDTO>>> SearchTimelineDeliveryByNameAsync(string name);
        Task<ApiResult<TimelineDeliveryDTO>> DeleteTimelineDeliveryAsync(int id);
        Task<ApiResult<TimelineDeliveryDTO>> UpdateTimelineDeliveryAsync(int id, TimelineDeliveryUpdateDTO updateDto);
        Task<ApiResult<TimelineDeliveryDTO>> CreateTimelineDeliveryAsync(TimelineDeliveryCreateDTO timelineDelivery);

    }
}

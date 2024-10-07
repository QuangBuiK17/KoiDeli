using KoiDeli.Domain.DTOs.OrderDetailDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IOrderDetailService
    {
        Task<ApiResult<List<OrderDetailDTO>>> GetOrderDetailsAsync();
        Task<ApiResult<OrderDetailDTO>> GetOrderDetailByIdAsync(int id);
        Task<ApiResult<List<OrderDetailDTO>>> SearchOrderDetailByNameAsync(string name);
        Task<ApiResult<OrderDetailDTO>> DeleteOrderDetailAsync(int id);
        Task<ApiResult<OrderDetailDTO>> UpdateOrderDetailAsync(int id, OrderDetailUpdateDTO updateDto);
        Task<ApiResult<OrderDetailDTO>> CreateOrderDetailAsync(OrderDetailCreateDTO orderDetail);

    }
}

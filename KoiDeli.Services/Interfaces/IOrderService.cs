using KoiDeli.Domain.DTOs.OrderDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResult<List<OrderDTO>>> GetOrdersAsync();
        Task<ApiResult<List<OrderDTO>>> GetOrdersEnableAsync();
        Task<ApiResult<OrderDTO>> GetOrderByIdAsync(int id);
        Task<ApiResult<List<OrderDTO>>> SearchOrderByNameAsync(string name);
        Task<ApiResult<OrderDTO>> DeleteOrderAsync(int id);
        Task<ApiResult<OrderDTO>> UpdateOrderAsync(int id, OrderUpdateDTO updateDto);
        Task<ApiResult<OrderDTO>> CreateOrderAsync(OrderCreateDTO order);


    }
}

using AutoMapper;
using KoiDeli.Domain.DTOs.OrderDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Domain.Enums;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public OrderService(
            IUnitOfWork unitOfWork,
            ICurrentTime currentTime,
            AppConfiguration configuration,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ApiResult<OrderDTO>> CreateOrderAsync(OrderCreateDTO orderDto)
        {
            var response = new ApiResult<OrderDTO>();

            try
            {
                // Ánh xạ OrderCreateDTO thành đối tượng Order
                var newOrder = _mapper.Map<Order>(orderDto);

                // Thiết lập trạng thái của đơn hàng
                newOrder.IsShipping = orderDto.IsShipping.HasValue
                    ? orderDto.IsShipping.Value.ToString()
                    : StatusEnum.Pending.ToString();

                // Thêm đơn hàng vào repository
                await _unitOfWork.OrderRepository.AddAsync(newOrder);

                // Lưu thay đổi vào cơ sở dữ liệu
                var saveResult = await _unitOfWork.SaveChangeAsync();
                if (saveResult > 0)
                {
                    // Ánh xạ Order thành OrderDTO để trả về
                    response.Data = _mapper.Map<OrderDTO>(newOrder);
                    response.Success = true;
                    response.Message = "Order created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create Order.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }


        public async Task<ApiResult<OrderDTO>> DeleteOrderAsync(int id)
        {
            var response = new ApiResult<OrderDTO>();
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);

            if (order != null)
            {
                _unitOfWork.OrderRepository.SoftRemove(order);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<OrderDTO>(order);
                    response.Success = true;
                    response.Message = "Order deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete Order.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Order not found.";
            }

            return response;
        }

        public async Task<ApiResult<OrderDTO>> GetOrderByIdAsync(int id)
        {
            var response = new ApiResult<OrderDTO>();

            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);

                if (order != null)
                {
                    response.Data = _mapper.Map<OrderDTO>(order);
                    response.Success = true;
                    response.Message = "Order retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Order not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<OrderDTO>>> GetOrdersAsync()
        {
            var response = new ApiResult<List<OrderDTO>>();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAllAsync();

                foreach (var order in orders)
                {
                    var orderDto = _mapper.Map<OrderDTO>(order);
                    orderDTOs.Add(orderDto);
                }

                if (orderDTOs.Count > 0)
                {
                    response.Data = orderDTOs;
                    response.Success = true;
                    response.Message = $"Found {orderDTOs.Count} Orders.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Orders found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<OrderDTO>>> SearchOrderByNameAsync(string name)
        {
            var response = new ApiResult<List<OrderDTO>>();
            List<OrderDTO> orderDTOs = new List<OrderDTO>();

            try
            {
                var orders = await _unitOfWork.OrderRepository.SearchAsync(o => o.URLCer1.Contains(name));

                foreach (var order in orders)
                {
                    var orderDto = _mapper.Map<OrderDTO>(order);
                    orderDTOs.Add(orderDto);
                }

                if (orderDTOs.Count > 0)
                {
                    response.Data = orderDTOs;
                    response.Success = true;
                    response.Message = $"{orderDTOs.Count} Orders found with the customer name '{name}'.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No Orders found with the customer name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<OrderDTO>> UpdateOrderAsync(int id, OrderUpdateDTO updateDto)
        {
            var response = new ApiResult<OrderDTO>();

            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);

                if (order != null)
                {
                    _mapper.Map(updateDto, order);
                    _unitOfWork.OrderRepository.Update(order);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<OrderDTO>(order);
                        response.Success = true;
                        response.Message = "Order updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update Order.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Order not found.";
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

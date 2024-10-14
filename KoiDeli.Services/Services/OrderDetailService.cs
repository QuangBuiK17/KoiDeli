using AutoMapper;
using KoiDeli.Domain.DTOs.OrderDetailDTOs;
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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public OrderDetailService(
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

        public async Task<ApiResult<OrderDetailDTO>> CreateOrderDetailAsync(OrderDetailCreateDTO orderDetailDto)
        {
            var response = new ApiResult<OrderDetailDTO>();

            try
            {
                var entity = _mapper.Map<OrderDetail>(orderDetailDto);

                entity.IsComplete = orderDetailDto.IsComplete.HasValue
                    ? orderDetailDto.IsComplete.Value.ToString()
                    : StatusEnum.Pending.ToString();

                await _unitOfWork.OrderDetailRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<OrderDetailDTO>(entity);
                    response.Success = true;
                    response.Message = "OrderDetail created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create OrderDetail.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<OrderDetailDTO>> DeleteOrderDetailAsync(int id)
        {
            var response = new ApiResult<OrderDetailDTO>();
            var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);

            if (orderDetail != null)
            {
                _unitOfWork.OrderDetailRepository.SoftRemove(orderDetail);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<OrderDetailDTO>(orderDetail);
                    response.Success = true;
                    response.Message = "OrderDetail deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete OrderDetail.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "OrderDetail not found.";
            }

            return response;
        }

        public async Task<ApiResult<OrderDetailDTO>> GetOrderDetailByIdAsync(int id)
        {
            var response = new ApiResult<OrderDetailDTO>();

            try
            {
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);

                if (orderDetail != null)
                {
                    response.Data = _mapper.Map<OrderDetailDTO>(orderDetail);
                    response.Success = true;
                    response.Message = "OrderDetail retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "OrderDetail not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<OrderDetailDTO>>> GetOrderDetailsAsync()
        {
            var response = new ApiResult<List<OrderDetailDTO>>();
            List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();

            try
            {
                var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync();

                foreach (var orderDetail in orderDetails)
                {
                    var orderDetailDto = _mapper.Map<OrderDetailDTO>(orderDetail);
                    orderDetailDTOs.Add(orderDetailDto);
                }

                if (orderDetailDTOs.Count > 0)
                {
                    response.Data = orderDetailDTOs;
                    response.Success = true;
                    response.Message = $"Found {orderDetailDTOs.Count} OrderDetails.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No OrderDetails found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<OrderDetailDTO>>> GetOrderDetailsEnableAsync()
        {
            var response = new ApiResult<List<OrderDetailDTO>>();
            List<OrderDetailDTO> orderDetailDTOs = new List<OrderDetailDTO>();

            try
            {
                var orderDetails = await _unitOfWork.OrderDetailRepository.SearchAsync(o => o.IsDeleted == false);

                foreach (var orderDetail in orderDetails)
                {
                    var orderDetailDto = _mapper.Map<OrderDetailDTO>(orderDetail);
                    orderDetailDTOs.Add(orderDetailDto);
                }

                if (orderDetailDTOs.Count > 0)
                {
                    response.Data = orderDetailDTOs;
                    response.Success = true;
                    response.Message = $"Found {orderDetailDTOs.Count} OrderDetails are being enable.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No OrderDetails are being enable.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public Task<ApiResult<List<OrderDetailDTO>>> SearchOrderDetailByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<OrderDetailDTO>> UpdateOrderDetailAsync(int id, OrderDetailUpdateDTO updateDto)
        {
            var response = new ApiResult<OrderDetailDTO>();

            try
            {
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);

                if (orderDetail != null)
                {
                    _mapper.Map(updateDto, orderDetail);
                    _unitOfWork.OrderDetailRepository.Update(orderDetail);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<OrderDetailDTO>(orderDetail);
                        response.Success = true;
                        response.Message = "OrderDetail updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update OrderDetail.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "OrderDetail not found.";
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

using AutoMapper;
using KoiDeli.Domain.DTOs.BoxOptionDTOs;
using KoiDeli.Domain.DTOs.OrderDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
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
    public class OrderTimelineService : IOrderTimelineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public OrderTimelineService(
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

        public async Task<ApiResult<OrderTimelineDTO>> CreateOrderTimelineAsync(OrderTimelineCreateDTO orderTimelineDto)
        {
            var response = new ApiResult<OrderTimelineDTO>();

            try
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
                    response.Message = "OrderTimeline created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create OrderTimeline.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<OrderTimelineDTO>> DeleteOrderTimelineAsync(int id)
        {
            var response = new ApiResult<OrderTimelineDTO>();
            var orderTimeline = await _unitOfWork.OrderTimelineRepository.GetByIdAsync(id);

            if (orderTimeline != null)
            {
                _unitOfWork.OrderTimelineRepository.SoftRemove(orderTimeline);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<OrderTimelineDTO>(orderTimeline);
                    response.Success = true;
                    response.Message = "OrderTimeline deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete OrderTimeline.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "OrderTimeline not found.";
            }

            return response;
        }

        public async Task<ApiResult<OrderTimelineDTO>> GetOrderTimelineByIdAsync(int id)
        {
            var response = new ApiResult<OrderTimelineDTO>();

            try
            {
                var orderTimeline = await _unitOfWork.OrderTimelineRepository.GetByIdAsync(id);

                if (orderTimeline != null)
                {
                    response.Data = _mapper.Map<OrderTimelineDTO>(orderTimeline);
                    response.Success = true;
                    response.Message = "OrderTimeline retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "OrderTimeline not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<OrderTimelineDTO>>> GetOrderTimelinesAsync()
        {
            var response = new ApiResult<List<OrderTimelineDTO>>();
            List<OrderTimelineDTO> orderTimelineDTOs = new List<OrderTimelineDTO>();

            try
            {
                var orderTimelines = await _unitOfWork.OrderTimelineRepository.GetAllAsync();

                foreach (var orderTimeline in orderTimelines)
                {
                    var orderTimelineDto = _mapper.Map<OrderTimelineDTO>(orderTimeline);
                    orderTimelineDTOs.Add(orderTimelineDto);
                }

                if (orderTimelineDTOs.Count > 0)
                {
                    response.Data = orderTimelineDTOs;
                    response.Success = true;
                    response.Message = $"Found {orderTimelineDTOs.Count} OrderTimelines.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No OrderTimelines found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<OrderTimelineDTO>>> SearchOrderTimelineByNameAsync(string name)
        {
            var response = new ApiResult<List<OrderTimelineDTO>>();
            List<OrderTimelineDTO> orderTimelineDTOs = new List<OrderTimelineDTO>();

            try
            {
                var orderTimelines = await _unitOfWork.OrderTimelineRepository.SearchAsync(o => o.Description.Contains(name));

                foreach (var orderTimeline in orderTimelines)
                {
                    var orderTimelineDto = _mapper.Map<OrderTimelineDTO>(orderTimeline);
                    orderTimelineDTOs.Add(orderTimelineDto);
                }

                if (orderTimelineDTOs.Count > 0)
                {
                    response.Data = orderTimelineDTOs;
                    response.Success = true;
                    response.Message = $"{orderTimelineDTOs.Count} OrderTimelines found with the name '{name}'.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No OrderTimelines found with the name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<OrderTimelineDTO>> UpdateOrderTimelineAsync(int id, OrderTimelineUpdateDTO updateDto)
        {
            var response = new ApiResult<OrderTimelineDTO>();

            try
            {
                var orderTimeline = await _unitOfWork.OrderTimelineRepository.GetByIdAsync(id);

                if (orderTimeline != null)
                {
                    _mapper.Map(updateDto, orderTimeline);
                    _unitOfWork.OrderTimelineRepository.Update(orderTimeline);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<OrderTimelineDTO>(orderTimeline);
                        response.Success = true;
                        response.Message = "OrderTimeline updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update OrderTimeline.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "OrderTimeline not found.";
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

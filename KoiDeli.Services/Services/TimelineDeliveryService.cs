using AutoMapper;
using KoiDeli.Domain.DTOs.OrderDetailDTOs;
using KoiDeli.Domain.DTOs.TimelineDeliveryDTOs;
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
    public class TimelineDeliveryService : ITimelineDeliveryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public TimelineDeliveryService(
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

        public async Task<ApiResult<TimelineDeliveryDTO>> CreateTimelineDeliveryAsync(TimelineDeliveryCreateDTO timelineDeliveryDto)
        {
            var response = new ApiResult<TimelineDeliveryDTO>();

            try
            {
                // Truy vấn các TimelineDelivery hiện tại của Vehicle và Branch
                var existingDeliveries = (await _unitOfWork.TimelineDeliveryRepository
                       .GetAllTimelineAsync())  // Lấy tất cả các dòng thời gian giao hàng
                       .Where(t => t.VehicleId == timelineDeliveryDto.VehicleId && t.BranchId == timelineDeliveryDto.BranchId)
                       .ToList();


                // Kiểm tra xung đột thời gian với các TimelineDelivery hiện có
                foreach (var delivery in existingDeliveries)
                {
                    if ((timelineDeliveryDto.StartDay < delivery.EndDay && timelineDeliveryDto.EndDay > delivery.StartDay))
                    {
                        response.Success = false;
                        response.Message = "The delivery schedule conflicts with an existing delivery for this vehicle and branch.";
                        return response;  // Trả về lỗi nếu có xung đột
                    }
                }

                // Nếu không có xung đột, tiến hành tạo mới TimelineDelivery
                var entity = _mapper.Map<TimelineDelivery>(timelineDeliveryDto);

                entity.IsCompleted = timelineDeliveryDto.IsCompleted.HasValue
                    ? timelineDeliveryDto.IsCompleted.Value.ToString()
                    : StatusEnum.Pending.ToString();

                await _unitOfWork.TimelineDeliveryRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<TimelineDeliveryDTO>(entity);
                    response.Success = true;
                    response.Message = "TimelineDelivery created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create TimelineDelivery.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }


        public async Task<ApiResult<TimelineDeliveryDTO>> DeleteTimelineDeliveryAsync(int id)
        {
            var response = new ApiResult<TimelineDeliveryDTO>();
            var timelineDelivery = await _unitOfWork.TimelineDeliveryRepository.GetByIdAsync(id);

            if (timelineDelivery != null)
            {
                _unitOfWork.TimelineDeliveryRepository.SoftRemove(timelineDelivery);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<TimelineDeliveryDTO>(timelineDelivery);
                    response.Success = true;
                    response.Message = "TimelineDelivery deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete TimelineDelivery.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "TimelineDelivery not found.";
            }

            return response;
        }

        public async Task<ApiResult<List<TimelineDeliveryDTO>>> GetTimelineDeliveriesAsync()
        {
            var response = new ApiResult<List<TimelineDeliveryDTO>>();
            List<TimelineDeliveryDTO> timelineDeliveryDTOs = new List<TimelineDeliveryDTO>();

            try
            {
                var timelineDeliveries = await _unitOfWork.TimelineDeliveryRepository.GetAllAsync();

                foreach (var timelineDelivery in timelineDeliveries)
                {
                    var timelineDeliveryDto = _mapper.Map<TimelineDeliveryDTO>(timelineDelivery);
                    timelineDeliveryDTOs.Add(timelineDeliveryDto);
                }

                if (timelineDeliveryDTOs.Count > 0)
                {
                    response.Data = timelineDeliveryDTOs;
                    response.Success = true;
                    response.Message = $"Found {timelineDeliveryDTOs.Count} TimelineDeliveries.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No TimelineDeliveries found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<TimelineDeliveryDTO>>> GetTimelineDeliveriesEnableAsync()
        {
            var response = new ApiResult<List<TimelineDeliveryDTO>>();
            List<TimelineDeliveryDTO> timelineDeliveryDTOs = new List<TimelineDeliveryDTO>();

            try
            {
                var timelineDeliveries = await _unitOfWork.TimelineDeliveryRepository.SearchAsync(o => o.IsDeleted == false);

                foreach (var timelineDelivery in timelineDeliveries)
                {
                    var timelineDeliveryDto = _mapper.Map<TimelineDeliveryDTO>(timelineDelivery);
                    timelineDeliveryDTOs.Add(timelineDeliveryDto);
                }

                if (timelineDeliveryDTOs.Count > 0)
                {
                    response.Data = timelineDeliveryDTOs;
                    response.Success = true;
                    response.Message = $"{timelineDeliveryDTOs.Count} TimelineDeliveries are being anable.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No TimelineDeliveries are being anable.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<TimelineDeliveryDTO>> GetTimelineDeliveryByIdAsync(int id)
        {
            var response = new ApiResult<TimelineDeliveryDTO>();

            try
            {
                var timelineDelivery = await _unitOfWork.TimelineDeliveryRepository.GetByIdAsync(id);

                if (timelineDelivery != null)
                {
                    response.Data = _mapper.Map<TimelineDeliveryDTO>(timelineDelivery);
                    response.Success = true;
                    response.Message = "TimelineDelivery retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "TimelineDelivery not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<TimelineDeliveryDTO>>> SearchTimelineDeliveryByNameAsync(string name)
        {
            var response = new ApiResult<List<TimelineDeliveryDTO>>();
            List<TimelineDeliveryDTO> timelineDeliveryDTOs = new List<TimelineDeliveryDTO>();

            try
            {
                var timelineDeliveries = await _unitOfWork.TimelineDeliveryRepository.SearchAsync(t => t.Description.Contains(name));

                foreach (var timelineDelivery in timelineDeliveries)
                {
                    var timelineDeliveryDto = _mapper.Map<TimelineDeliveryDTO>(timelineDelivery);
                    timelineDeliveryDTOs.Add(timelineDeliveryDto);
                }

                if (timelineDeliveryDTOs.Count > 0)
                {
                    response.Data = timelineDeliveryDTOs;
                    response.Success = true;
                    response.Message = $"{timelineDeliveryDTOs.Count} TimelineDeliveries found with the name '{name}'.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No TimelineDeliveries found with the name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<TimelineDeliveryDTO>> UpdateTimelineDeliveryAsync(int id, TimelineDeliveryUpdateDTO updateDto)
        {
            var response = new ApiResult<TimelineDeliveryDTO>();

            try
            {
                var timelineDelivery = await _unitOfWork.TimelineDeliveryRepository.GetByIdAsync(id);

                if (timelineDelivery != null)
                {
                    _mapper.Map(updateDto, timelineDelivery);
                    _unitOfWork.TimelineDeliveryRepository.Update(timelineDelivery);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<TimelineDeliveryDTO>(timelineDelivery);
                        response.Success = true;
                        response.Message = "TimelineDelivery updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update TimelineDelivery.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "TimelineDelivery not found.";
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

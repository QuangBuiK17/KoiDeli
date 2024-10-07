using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Azure;
using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUnitOfWork = KoiDeli.Repositories.Interfaces.IUnitOfWork;

namespace KoiDeli.Services.Services
{
    public class DistanceService : IDistanceService
    {
        private readonly IUnitOfWork  _unitOfWork;
        private readonly IMapper _mapper;

        public DistanceService(IUnitOfWork unitOfWork,
                           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResult<DistanceDTO>> CreateDistanceAsync(DistanceCreateDTO createDTO)
        {
            var response = new ApiResult<DistanceDTO>();
            try
            {
                var distance = _mapper.Map<Distance>(createDTO);
                await _unitOfWork.DistanceRepository.AddAsync(distance);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<DistanceDTO>(distance);
                    response.Success = true;
                    response.Message = "Create new Distance successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Create new Distance fail";
                    return response;
                }
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }

        public async Task<ApiResult<DistanceDTO>> DeleteDistanceAsync(int id)
        {
            var response = new ApiResult<DistanceDTO>();
            var distance = await _unitOfWork.DistanceRepository.GetByIdAsync(id);
            if (distance == null)
            {
                response.Success = false;
                response.Message = "Distance not found!";
                return response;
            }
            else 
            {
                _unitOfWork.DistanceRepository.SoftRemove(distance);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<DistanceDTO>(distance);
                    response.Success = true;
                    response.Message = "Delete Distance successfully";
                    return response;
                }
                else 
                {
                    response.Success = false;
                    response.Message = "Delete Distance fail";
                    return response;
                }
            }
        }

        

        public async Task<ApiResult<List<DistanceDTO>>> GetDistancesAsync()
        {
            var response = new ApiResult<List<DistanceDTO>>();
            List<DistanceDTO> DistancesDTOs = new List<DistanceDTO>();
            try
            {
                var distances = await _unitOfWork.DistanceRepository.GetAllAsync();
                foreach (var distance in distances) 
                {
                    var distanceDto = _mapper.Map<DistanceDTO>(distance);
                    distanceDto.RangeDistance = distance.RangeDistance;
                    distanceDto.Price = distance.Price;
                    DistancesDTOs.Add(distanceDto);
                }
                if (DistancesDTOs.Count > 0)
                {
                    response.Data = DistancesDTOs;
                    response.Success = true;
                    response.Message = $"Have {DistancesDTOs.Count} distances.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No distance created";
                    response.Error = "No error";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = "Exception";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;

        }

        public async Task<ApiResult<List<DistanceDTO>>> GetDistancesEnabledAsync()
        {
            var response = new ApiResult<List<DistanceDTO>>();
            List<DistanceDTO> DistancesDTOs = new List<DistanceDTO>();
            try
            {
                var distances = await _unitOfWork.DistanceRepository.GetDistanceEnabledAsync();
                foreach (var distance in distances)
                {
                    var distanceDto = _mapper.Map<DistanceDTO>(distance);
                    distanceDto.RangeDistance = distance.RangeDistance;
                    distanceDto.Price = distance.Price;
                    DistancesDTOs.Add(distanceDto);
                }
                if (DistancesDTOs.Count > 0)
                {
                    response.Data = DistancesDTOs;
                    response.Success = true;
                    response.Message = $"Have {DistancesDTOs.Count} distances enabled.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No distance created";
                    response.Error = "No error";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = "Exception";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ApiResult<DistanceDTO>> UpdateDistanceAsync(int id, DistanceUpdateDTO updateDto)
        {
            var response = new ApiResult<DistanceDTO>();

            try
            {
                var enityById = await _unitOfWork.DistanceRepository.GetByIdAsync(id);

                if (enityById != null)
                {
                    var newb = _mapper.Map(updateDto, enityById);
                    var bAfter = _mapper.Map<Distance>(newb);
                    _unitOfWork.DistanceRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        response.Data = _mapper.Map<DistanceDTO>(bAfter);
                        response.Message = $"Successfull for update Distance.";
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Error = "Save update failed";
                        return response;
                    }

                }
                else
                {
                    response.Success = false;
                    response.Message = $"Have no Distance.";
                    response.Error = "Not have a Distance";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }
        public async Task<ApiResult<DistanceDTO>> GetDistanceByIdAsync(int id)
        {
            //throw new NotImplementedException();
            var response = new ApiResult<DistanceDTO>();
            try
            {
                var distance = await _unitOfWork.DistanceRepository.GetByIdAsync(id);
                if(distance == null)
                {
                    response.Success = false;
                    response.Message = "Distance ID doesn't exit!";
                    return response;
                }
                else
                {
                    response.Data = _mapper.Map<DistanceDTO>(distance);
                    response.Success = true;
                    response.Message = "Distance ID is valid";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }
    }
}

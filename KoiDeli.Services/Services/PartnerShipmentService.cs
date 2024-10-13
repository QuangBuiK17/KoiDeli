using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Azure;
using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Domain.Enums;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUnitOfWork = KoiDeli.Repositories.Interfaces.IUnitOfWork;

namespace KoiDeli.Services.Services
{
    public class PartnerShipmentService : IPartnerShipmentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public PartnerShipmentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<PartnerDTO>> CreatePartnerAsync(PartnerCreateDTO createDTO)
        {
            var response = new ApiResult<PartnerDTO>();
            try
            {
                var partner = _mapper.Map<PartnerShipment>(createDTO);

                partner.IsCompleted = createDTO.IsCompleted.HasValue
                    ? createDTO.IsCompleted.Value.ToString()
                    : StatusEnum.Pending.ToString();

                await _unitOfWork.PartnerShipmentRepository.AddAsync(partner);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<PartnerDTO>(partner);
                    response.Success = true;
                    response.Message = "Create new Partner successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Create new Partner fail";
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

        public async Task<ApiResult<PartnerDTO>> DeletePartnerAsync(int id)
        {
            var response = new ApiResult<PartnerDTO>();
            var partner = await _unitOfWork.PartnerShipmentRepository.GetByIdAsync(id);
            if (partner == null)
            {
                response.Success = false;
                response.Message = "Partner not found!";
                return response;
            }
            else 
            {
                _unitOfWork.PartnerShipmentRepository.SoftRemove(partner);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<PartnerDTO>(partner);
                    response.Success = true;
                    response.Message = "Delete partner successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Delete partner fail";
                    return response;
                }
                
            }
        }

        public async Task<ApiResult<PartnerDTO>> GetPartnerByIdAsync(int id)
        {
            //throw new NotImplementedException();
            var response = new ApiResult<PartnerDTO>();
            try
            {
                var partner = await _unitOfWork.PartnerShipmentRepository.GetByIdAsync(id);
                if (partner == null)
                {
                    response.Success = false;
                    response.Message = "partner ID doesn't exit!";
                    return response;
                }
                else
                {
                    response.Data = _mapper.Map<PartnerDTO>(partner);
                    response.Success = true;
                    response.Message = "Partner ID is valid";
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

        public async Task<ApiResult<List<PartnerDTO>>> GetPartnersAsync()
        {
            var response = new ApiResult<List<PartnerDTO>>();
            List<PartnerDTO> PartnerDTOs = new List<PartnerDTO>();
            try
            {
                var partners = await _unitOfWork.PartnerShipmentRepository.GetAllAsync();
                foreach (var partner in partners)
                {
                    var partnerDto= _mapper.Map<PartnerDTO>(partner);
                    // distanceDto.RangeDistance = distance.RangeDistance;
                    //distanceDto.Price = distance.Price;
                    PartnerDTOs.Add(partnerDto);
                }
                if (PartnerDTOs.Count > 0)
                {
                    response.Data = PartnerDTOs;
                    response.Success = true;
                    response.Message = $"Have {PartnerDTOs.Count} partner.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No partner created";
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

        public async Task<ApiResult<List<PartnerDTO>>> GetPartnersByNameAsync(string name)
        {
            var response = new ApiResult<List<PartnerDTO>>();
            List<PartnerDTO> PartnerDTOs = new List<PartnerDTO>();
            try
            {
                var partners = await _unitOfWork.PartnerShipmentRepository.GetPartnerByNameAsync(name);
                foreach (var partner in partners)
                {
                    var partnerDto = _mapper.Map<PartnerDTO>(partner);
                    PartnerDTOs.Add(partnerDto);
                }
                if (PartnerDTOs.Count > 0)
                {
                    response.Data = PartnerDTOs;
                    response.Success = true;
                    response.Message = $"Have {PartnerDTOs.Count} partner enabled.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "Name not found or have been deleted";
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

        public async Task<ApiResult<List<PartnerDTO>>> GetPartnersEnabledAsync()
        {
            var response = new ApiResult<List<PartnerDTO>>();
            List<PartnerDTO> PartnerDTOs = new List<PartnerDTO>();
            try
            {
                var partners = await _unitOfWork.PartnerShipmentRepository.GetPartnerEnabledAsync();
                foreach (var partner in partners)
                {
                    var partnerDto = _mapper.Map<PartnerDTO>(partner);
                    // distanceDto.RangeDistance = distance.RangeDistance;
                    //distanceDto.Price = distance.Price;
                    PartnerDTOs.Add(partnerDto);
                }
                if (PartnerDTOs.Count > 0)
                {
                    response.Data = PartnerDTOs;
                    response.Success = true;
                    response.Message = $"Have {PartnerDTOs.Count} partner enabled.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No partner created";
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

        public async Task<ApiResult<PartnerDTO>> UpdatePartnerAsync(int id, PartnerUpdateDTO updateDto)
        {
            var response = new ApiResult<PartnerDTO>();

            try
            {
                var enityById = await _unitOfWork.PartnerShipmentRepository.GetByIdAsync(id);

                if (enityById != null)
                {
                    var newb = _mapper.Map(updateDto, enityById);
                    var bAfter = _mapper.Map<PartnerShipment>(newb);
                    _unitOfWork.PartnerShipmentRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        response.Data = _mapper.Map<PartnerDTO>(bAfter);
                        response.Message = $"Successfull for update partner.";
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
                    response.Message = $"Have no partner.";
                    response.Error = "Not have a partner";
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

        public async Task<ApiResult<PartnerDTO>> UpdatePartnerCompleteAsync(int id, PartnerUpdateCompleteDTO updateDto)
        {
            var response = new ApiResult<PartnerDTO>();

            try
            {
                var enityById = await _unitOfWork.PartnerShipmentRepository.GetByIdAsync(id);

                if (enityById != null)
                {
                    var newb = _mapper.Map(updateDto, enityById);
                    var bAfter = _mapper.Map<PartnerShipment>(newb);
                    _unitOfWork.PartnerShipmentRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        response.Data = _mapper.Map<PartnerDTO>(bAfter);
                        response.Message = $"Successfull for update partner status complete.";
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
                    response.Message = $"Have no partner.";
                    response.Error = "Not have a partner";
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

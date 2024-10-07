<<<<<<< Updated upstream
﻿using AutoMapper;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
=======
﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Azure;
using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
>>>>>>> Stashed changes
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
<<<<<<< Updated upstream
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUnitOfWork = KoiDeli.Repositories.Interfaces.IUnitOfWork;
>>>>>>> Stashed changes

namespace KoiDeli.Services.Services
{
    public class PartnerShipmentService : IPartnerShipmentService
    {
<<<<<<< Updated upstream
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public PartnerShipmentService(
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

        public async Task<ApiResult<PartnerShipmentDTO>> CreatePartnerShipmentAsync(PartnerShipmentCreateDTO partnerShipmentDto)
        {
            var response = new ApiResult<PartnerShipmentDTO>();

            try
            {
                var entity = _mapper.Map<PartnerShipment>(partnerShipmentDto);

                await _unitOfWork.PartnerShipmentRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<PartnerShipmentDTO>(entity);
                    response.Success = true;
                    response.Message = "PartnerShipment created successfully.";
=======
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
                await _unitOfWork.PartnerShipmentRepository.AddAsync(partner);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<PartnerDTO>(partner);
                    response.Success = true;
                    response.Message = "Create new Partner successfully";
                    return response;
>>>>>>> Stashed changes
                }
                else
                {
                    response.Success = false;
<<<<<<< Updated upstream
                    response.Message = "Failed to create PartnerShipment.";
=======
                    response.Message = "Create new Partner fail";
                    return response;
>>>>>>> Stashed changes
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
<<<<<<< Updated upstream
            }

            return response;
        }

        public async Task<ApiResult<PartnerShipmentDTO>> DeletePartnerShipmentAsync(int id)
        {
            var response = new ApiResult<PartnerShipmentDTO>();
            var partnerShipment = await _unitOfWork.PartnerShipmentRepository.GetByIdAsync(id);

            if (partnerShipment != null)
            {
                _unitOfWork.PartnerShipmentRepository.SoftRemove(partnerShipment);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<PartnerShipmentDTO>(partnerShipment);
                    response.Success = true;
                    response.Message = "PartnerShipment deleted successfully.";
=======
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
>>>>>>> Stashed changes
                }
                else
                {
                    response.Success = false;
<<<<<<< Updated upstream
                    response.Message = "Failed to delete PartnerShipment.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "PartnerShipment not found.";
            }

            return response;
        }

        public async Task<ApiResult<PartnerShipmentDTO>> GetPartnerShipmentByIdAsync(int id)
        {
            var response = new ApiResult<PartnerShipmentDTO>();

            try
            {
                var partnerShipment = await _unitOfWork.PartnerShipmentRepository.GetByIdAsync(id);

                if (partnerShipment != null)
                {
                    response.Data = _mapper.Map<PartnerShipmentDTO>(partnerShipment);
                    response.Success = true;
                    response.Message = "PartnerShipment retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "PartnerShipment not found.";
=======
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
>>>>>>> Stashed changes
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
<<<<<<< Updated upstream
            }

            return response;
        }

        public async Task<ApiResult<List<PartnerShipmentDTO>>> GetPartnerShipmentsAsync()
        {
            var response = new ApiResult<List<PartnerShipmentDTO>>();
            List<PartnerShipmentDTO> partnerShipmentDTOs = new List<PartnerShipmentDTO>();

            try
            {
                var partnerShipments = await _unitOfWork.PartnerShipmentRepository.GetAllAsync();

                foreach (var partnerShipment in partnerShipments)
                {
                    var partnerShipmentDto = _mapper.Map<PartnerShipmentDTO>(partnerShipment);
                    partnerShipmentDTOs.Add(partnerShipmentDto);
                }

                if (partnerShipmentDTOs.Count > 0)
                {
                    response.Data = partnerShipmentDTOs;
                    response.Success = true;
                    response.Message = $"Found {partnerShipmentDTOs.Count} PartnerShipments.";
=======
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

>>>>>>> Stashed changes
                }
                else
                {
                    response.Success = false;
<<<<<<< Updated upstream
                    response.Message = "No PartnerShipments found.";
=======
                    response.Message = "No partner created";
                    response.Error = "No error";
>>>>>>> Stashed changes
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
<<<<<<< Updated upstream
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<PartnerShipmentDTO>>> SearchPartnerShipmentByNameAsync(string name)
        {
            var response = new ApiResult<List<PartnerShipmentDTO>>();
            List<PartnerShipmentDTO> partnerShipmentDTOs = new List<PartnerShipmentDTO>();

            try
            {
                var partnerShipments = await _unitOfWork.PartnerShipmentRepository.SearchAsync(p => p.Name.Contains(name));

                foreach (var partnerShipment in partnerShipments)
                {
                    var partnerShipmentDto = _mapper.Map<PartnerShipmentDTO>(partnerShipment);
                    partnerShipmentDTOs.Add(partnerShipmentDto);
                }

                if (partnerShipmentDTOs.Count > 0)
                {
                    response.Data = partnerShipmentDTOs;
                    response.Success = true;
                    response.Message = $"{partnerShipmentDTOs.Count} PartnerShipments found with the name '{name}'.";
=======
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

>>>>>>> Stashed changes
                }
                else
                {
                    response.Success = false;
<<<<<<< Updated upstream
                    response.Message = $"No PartnerShipments found with the name '{name}'.";
=======
                    response.Message = "No partner created";
                    response.Error = "No error";
>>>>>>> Stashed changes
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
<<<<<<< Updated upstream
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<PartnerShipmentDTO>> UpdatePartnerShipmentAsync(int id, PartnerShipmentUpdateDTO updateDto)
        {
            var response = new ApiResult<PartnerShipmentDTO>();

            try
            {
                var partnerShipment = await _unitOfWork.PartnerShipmentRepository.GetByIdAsync(id);

                if (partnerShipment != null)
                {
                    _mapper.Map(updateDto, partnerShipment);
                    _unitOfWork.PartnerShipmentRepository.Update(partnerShipment);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<PartnerShipmentDTO>(partnerShipment);
                        response.Success = true;
                        response.Message = "PartnerShipment updated successfully.";
=======
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
>>>>>>> Stashed changes
                    }
                    else
                    {
                        response.Success = false;
<<<<<<< Updated upstream
                        response.Message = "Failed to update PartnerShipment.";
                    }
=======
                        response.Error = "Save update failed";
                        return response;
                    }

>>>>>>> Stashed changes
                }
                else
                {
                    response.Success = false;
<<<<<<< Updated upstream
                    response.Message = "PartnerShipment not found.";
                }
=======
                    response.Message = $"Have no partner.";
                    response.Error = "Not have a partner";
                    return response;
                }

>>>>>>> Stashed changes
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
<<<<<<< Updated upstream
            }

            return response;
        }

=======
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
>>>>>>> Stashed changes
    }
}

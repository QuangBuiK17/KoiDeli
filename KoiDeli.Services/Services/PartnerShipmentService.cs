using AutoMapper;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Domain.Entities;
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
    public class PartnerShipmentService : IPartnerShipmentService
    {
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
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create PartnerShipment.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
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
                }
                else
                {
                    response.Success = false;
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
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
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
                }
                else
                {
                    response.Success = false;
                    response.Message = "No PartnerShipments found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
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
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No PartnerShipments found with the name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
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
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update PartnerShipment.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "PartnerShipment not found.";
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

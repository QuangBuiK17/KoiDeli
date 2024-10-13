using AutoMapper;
using KoiDeli.Domain.DTOs.VehicleDTOs;
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
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public VehicleService(
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

        public async Task<ApiResult<VehicleDTO>> CreateVehicleAsync(VehicleCreateDTO vehicleDto)
        {
            var response = new ApiResult<VehicleDTO>();

            try
            {
                var entity = _mapper.Map<Vehicle>(vehicleDto);

                await _unitOfWork.VehicleRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<VehicleDTO>(entity);
                    response.Success = true;
                    response.Message = "Vehicle created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create Vehicle.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<VehicleDTO>> DeleteVehicleAsync(int id)
        {
            var response = new ApiResult<VehicleDTO>();
            var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(id);

            if (vehicle != null)
            {
                _unitOfWork.VehicleRepository.SoftRemove(vehicle);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<VehicleDTO>(vehicle);
                    response.Success = true;
                    response.Message = "Vehicle deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete Vehicle.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Vehicle not found.";
            }

            return response;
        }

        public async Task<ApiResult<VehicleDTO>> GetVehicleByIdAsync(int id)
        {
            var response = new ApiResult<VehicleDTO>();

            try
            {
                var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(id);

                if (vehicle != null)
                {
                    response.Data = _mapper.Map<VehicleDTO>(vehicle);
                    response.Success = true;
                    response.Message = "Vehicle retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Vehicle not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<VehicleDTO>>> GetVehiclesAsync()
        {
            var response = new ApiResult<List<VehicleDTO>>();
            List<VehicleDTO> vehicleDTOs = new List<VehicleDTO>();

            try
            {
                var vehicles = await _unitOfWork.VehicleRepository.GetAllAsync();

                foreach (var vehicle in vehicles)
                {
                    var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);
                    vehicleDTOs.Add(vehicleDto);
                }

                if (vehicleDTOs.Count > 0)
                {
                    response.Data = vehicleDTOs;
                    response.Success = true;
                    response.Message = $"Found {vehicleDTOs.Count} Vehicles.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Vehicles found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<VehicleDTO>>> GetVehiclesEnableAsync()
        {
            var response = new ApiResult<List<VehicleDTO>>();
            List<VehicleDTO> vehicleDTOs = new List<VehicleDTO>();

            try
            {
                var vehicles = await _unitOfWork.VehicleRepository.SearchAsync(v => v.IsDeleted == false);

                foreach (var vehicle in vehicles)
                {
                    var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);
                    vehicleDTOs.Add(vehicleDto);
                }

                if (vehicleDTOs.Count > 0)
                {
                    response.Data = vehicleDTOs;
                    response.Success = true;
                    response.Message = $"{vehicleDTOs.Count} Vehicles are being enable.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No Vehicles are being enable.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<VehicleDTO>>> SearchVehicleByNameAsync(string name)
        {
            var response = new ApiResult<List<VehicleDTO>>();
            List<VehicleDTO> vehicleDTOs = new List<VehicleDTO>();

            try
            {
                var vehicles = await _unitOfWork.VehicleRepository.SearchAsync(v => v.Name.Contains(name));

                foreach (var vehicle in vehicles)
                {
                    var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);
                    vehicleDTOs.Add(vehicleDto);
                }

                if (vehicleDTOs.Count > 0)
                {
                    response.Data = vehicleDTOs;
                    response.Success = true;
                    response.Message = $"{vehicleDTOs.Count} Vehicles found with the name '{name}'.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No Vehicles found with the name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<VehicleDTO>> UpdateVehicleAsync(int id, VehicleUpdateDTO updateDto)
        {
            var response = new ApiResult<VehicleDTO>();

            try
            {
                var vehicle = await _unitOfWork.VehicleRepository.GetByIdAsync(id);

                if (vehicle != null)
                {
                    _mapper.Map(updateDto, vehicle);
                    _unitOfWork.VehicleRepository.Update(vehicle);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<VehicleDTO>(vehicle);
                        response.Success = true;
                        response.Message = "Vehicle updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update Vehicle.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Vehicle not found.";
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

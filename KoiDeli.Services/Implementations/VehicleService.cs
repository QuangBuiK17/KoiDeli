using AutoMapper;
using KoiDeli.Domain.DTOs.VehicleDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Implementations;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Implementations
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository  _vehicleRepository;
        private readonly IMapper _mapper;
        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VehicleDTO>>(vehicles);
        }
        public async Task<VehicleDTO> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            return _mapper.Map<VehicleDTO>(vehicle);
        }

        public async Task AddVehicleAsync(VehicleDTO vehicleDto)
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            await _vehicleRepository.AddAsync(vehicle);
        }
        public async Task UpdateVehicleAsync(VehicleDTO vehicleDto)
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleDto);
            await _vehicleRepository.UpdateAsync(vehicle);
        }
        public async Task DeleteVehicleAsync(int id)
        {
            await _vehicleRepository.DeleteAsync(id);
        }
    }
}

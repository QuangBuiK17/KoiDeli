using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Implementations
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly KoiDeliDbContext _context;
        private readonly ICurrentTime _currentTime;
        public VehicleRepository(KoiDeliDbContext context, ICurrentTime currentTime)
        {
            _context = context;
            _currentTime = currentTime;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.Where(v => !v.IsDeleted).ToListAsync();
        }

        public async Task<Vehicle> GetByIdAsync(int id)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            vehicle.CreationDate = _currentTime.GetCurrentTime();
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            vehicle.ModificationDate = _currentTime.GetCurrentTime();
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var vehicle = await GetByIdAsync(id);
            if (vehicle != null)
            {
                vehicle.IsDeleted = true;
                vehicle.DeletionDate = _currentTime.GetCurrentTime();
                await _context.SaveChangesAsync();
            }
        }
    }
}

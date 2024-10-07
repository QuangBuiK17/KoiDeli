using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
<<<<<<< Updated upstream
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
=======
    public interface IVehicleRepository 
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> GetByIdAsync(int id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(int id);

>>>>>>> Stashed changes
    }
}

using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface IBoxRepository : IGenericRepository<Box>
    {
        Task<List<Box>> GetBoxsEnabledAsync();
        Task<List<Box>> GetRBoxsByNameAsync(string name);
    }
}

using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface IPartnerShipmentRepository : IGenericRepository<PartnerShipment>
    {
        Task<List<PartnerShipment>> GetPartnerEnabledAsync();
        Task<List<PartnerShipment>> GetPartnerByNameAsync(string name);
    }
}
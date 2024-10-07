using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Repositories
{
    public class PartnerShipmentRepository : GenericRepository<PartnerShipment>, IPartnerShipmentRepository
    {
        private readonly KoiDeliDbContext _dbcontext;
        public PartnerShipmentRepository(KoiDeliDbContext context,
                                        ICurrentTime timeService,
                                        IClaimsService claimsService) 
            : base(context, timeService, claimsService)
        {
            _dbcontext = context;
        }

        public async Task<List<PartnerShipment>> GetPartnerEnabledAsync()
        {
            var data = await _dbcontext.PartnerShipment.Where(d => d.IsDeleted == false).ToListAsync();
            if (data.Count == 0 || data == null)
            {
                return null;
            }
            return data;
        }
    }
}

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
    public class DistanceRepository : GenericRepository<Distance>, IDistanceRepository
    {
        private readonly KoiDeliDbContext _dbContext;
        public DistanceRepository(KoiDeliDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService) 
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Distance>> GetDistanceEnabledAsync()
        {
            var data = await _dbContext.Distances.Where(d => d.IsDeleted == false).ToListAsync();
            if (data.Count == 0 || data == null)
            {
                return null;
            }
            return data;
        }
    }
}

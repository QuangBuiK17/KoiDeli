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
    public class BoxRepository : GenericRepository<Box>, IBoxRepository
    {
        private readonly KoiDeliDbContext _dbContext;
        public BoxRepository(
            KoiDeliDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Box>> GetBoxsEnabledAsync()
        {
            var data = await _dbContext.Boxes.Where(r => r.IsDeleted == false).ToListAsync();
            if (data.Count == 0 || data == null)
            {
                return null;
            }
            return data;
        }

        public async Task<List<Box>> GetRBoxsByNameAsync(string name)
        {
            var data = await _dbContext.Boxes
                .Where(d => !d.IsDeleted && d.Name.Contains(name))
                .ToListAsync();

            return data.Any() ? data : new List<Box>();
        }
    }
}

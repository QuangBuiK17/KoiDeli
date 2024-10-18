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
    public class TimelineDeliveryRepository : GenericRepository<TimelineDelivery>, ITimelineDeliveryRepository
    {
        private readonly KoiDeliDbContext _dbContext;
        public TimelineDeliveryRepository(
            KoiDeliDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<TimelineDelivery>> GetAllTimelineAsync()
        {
            return await _dbContext.TimelineDelivery
                .Include(td => td.Vehicle)  // Include thông tin về Vehicle
                .Include(td => td.Branch)   // Include thông tin về Branch
                .Include(td => td.OrderTimelines)  // Include thông tin liên quan đến OrderTimeline nếu cần
                .ToListAsync();
        }
    }
}

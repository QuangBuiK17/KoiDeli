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
    public class BoxOptionRepository : GenericRepository<BoxOption>, IBoxOptionRepository
    {
        private readonly KoiDeliDbContext _dbContext;
        public BoxOptionRepository(
            KoiDeliDbContext context,
            ICurrentTime timeService,
            IClaimsService claimsService
        )
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public IQueryable<BoxOption> GetAll()
        {
            return _dbContext.Set<BoxOption>().AsQueryable();
        }
    }
}    

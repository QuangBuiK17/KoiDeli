using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly KoiDeliDbContext _context;
        public UserRepository(KoiDeliDbContext context,
                              ICurrentTime timeService,
                              IClaimsService claimsService) 
            : base(context, timeService, claimsService)
        {
            _context = context;
        }
    }
}

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
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly KoiDeliDbContext _dbContext;
        public RoleRepository(KoiDeliDbContext context, 
                              ICurrentTime timeService, 
                              IClaimsService claimsService) 
            : base(context, timeService, claimsService)
        {
            _dbContext = context;
        }

        public async Task<List<Role>> GetRoleByNameAsync(string name)
        {
            var data = await _dbContext.Roles
                .Where(d => !d.IsDeleted && d.Name.Contains(name))
                .ToListAsync();

            return data.Any() ? data : new List<Role>();
        }

        public async Task<List<Role>> GetRolesEnabledAsync()
        {
            var data = await _dbContext.Roles.Where(r => r.IsDeleted == false).ToListAsync();
            if (data.Count == 0 || data == null)
            {
                return null;
            }
            return data;
        }
    }
}

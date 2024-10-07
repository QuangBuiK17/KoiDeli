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
    public class WalletRepository : GenericRepository<Wallet>, IWalletRepository
    {
        private readonly KoiDeliDbContext _context;
        public WalletRepository(KoiDeliDbContext context,
                                ICurrentTime timeService,
                                IClaimsService claimsService) 
            : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<bool> CheckUserIdExisted(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<List<Wallet>> GetWalletssEnabledAsync()
        {
            var data = await _context.Wallets.Where(w => w.IsDeleted == false).ToListAsync();
            if (data.Count == 0 || data == null)
            {
                return null;
            }
            return data;
        }

    }
}

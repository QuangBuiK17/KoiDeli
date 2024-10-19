using KoiDeli.Domain.DTOs.UserDTOs;
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

        public async Task<RoleInfoModel> GetRole(User user)
        {
            // Kiểm tra nếu user có RoleId được gán
            if (user.RoleId == null)
            {
                throw new Exception("User does not have a role assigned.");
            }

            // Lấy thông tin của vai trò dựa trên RoleId từ bảng Roles
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (role == null)
            {
                throw new Exception("Role not found.");
            }

            // Trả về thông tin vai trò
            return new RoleInfoModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
        }


        public async Task<List<User>> GetUsersByNameAsync(string name)
        {
            var data = await _context.Users
                .Where(d => !d.IsDeleted && d.Name.Contains(name))
                .ToListAsync();

            return data.Any() ? data : new List<User>();
        }

        public async Task<List<User>> GetUsersEnabledAsync()
        {
            var data = await _context.Users.Where(r => r.IsDeleted == false).ToListAsync();
            if (data.Count == 0 || data == null)
            {
                return null;
            }
            return data;
        }
    }
}

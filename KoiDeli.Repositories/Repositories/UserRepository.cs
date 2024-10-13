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
            // Giả sử bạn có một bảng `UserRoles` trong cơ sở dữ liệu lưu thông tin người dùng và vai trò của họ
            // Bạn có thể lấy vai trò của user từ bảng `UserRoles` và sau đó lấy thông tin của vai trò từ `Role` bảng.

            var userRole = await _context.Roles.FirstOrDefaultAsync(ur => ur.Id == user.Id);
            if (userRole == null)
            {
                throw new Exception("User does not have a role assigned.");
            }

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == userRole.Id);
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
    }
}

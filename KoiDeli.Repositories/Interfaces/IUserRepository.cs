using KoiDeli.Domain.DTOs.UserDTOs;
using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<RoleInfoModel> GetRole(User user);


        Task<List<User>> GetUsersEnabledAsync();
        Task<List<User>> GetUsersByNameAsync(string name);
    }
}

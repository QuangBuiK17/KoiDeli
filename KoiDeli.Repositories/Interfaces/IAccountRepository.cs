using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface IAccountRepository :IGenericRepository<User>
    {
        Task<User> GetUserByEmailAndPasswordHash(string email, string passwordHash);
        Task<User> CheckEmailNameExisted(string email);
        Task<bool> CheckPhoneNumberExisted(string phonenumber);
        Task<User> GetUserByConfirmationToken(string token);
        Task<IEnumerable<User>> SearchAccountByNameAsync(string name);
        Task<IEnumerable<User>> GetAccountsAsync();
        Task<IEnumerable<User>> SearchAccountByRoleNameAsync(string roleName);
        Task<IEnumerable<User>> GetSortedAccountAsync();
        Task<Role> GetRoleNameByRoleId(int RoleId);
    }
}

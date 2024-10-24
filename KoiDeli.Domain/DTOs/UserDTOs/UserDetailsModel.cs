using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.UserDTOs
{
    public class UserDetailsModel
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? ConfirmationToken { get; set; }
        public bool IsConfirmed { get; set; }
        public string? UrlAvatar { get; set; }
        public string? Address { get; set; }
        public RoleInfoModel? Role {  get; set; }
    }
}

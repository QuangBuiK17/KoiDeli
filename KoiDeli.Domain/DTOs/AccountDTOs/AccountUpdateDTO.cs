using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.AccountDTOs
{
    public class AccountUpdateDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public int? RoleID { get; set; }
        public string? Gender { get; set; }
        public bool IsConfirmed { get; set; }
    }
}

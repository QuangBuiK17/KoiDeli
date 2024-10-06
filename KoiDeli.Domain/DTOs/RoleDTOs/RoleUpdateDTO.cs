using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.RoleDTOs
{
    public class RoleUpdateDTO
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? RoleName { get; set; }
    }
}

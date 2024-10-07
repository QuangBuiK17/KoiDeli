using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.KoiFishDTOs
{
    public class KoiFishUpdateDTO
    {
        public Int64 Size { get; set; }
        public Int64 Volume { get; set; }
        public string? Description { get; set; }
    }
}

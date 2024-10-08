using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.KoiFishDTOs
{
    public class KoiFishDTO
    {
        public int Id { get; set; }
        public string? Size { get; set; }
        public Int64 Volume { get; set; }
        public string? Description { get; set; }
    }
}

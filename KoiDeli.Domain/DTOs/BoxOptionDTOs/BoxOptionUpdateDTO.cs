using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxOptionDTOs
{
    public class BoxOptionUpdateDTO
    {
        public Int64 Quantity { get; set; }
        public string? Description { get; set; }
        public StatusEnum? IsChecked { get; set; } 
    }
}

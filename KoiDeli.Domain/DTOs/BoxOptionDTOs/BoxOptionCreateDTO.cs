﻿using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxOptionDTOs
{
    public class BoxOptionCreateDTO
    {
        public int Id { get; set; }
        public int FishId { get; set; }
        public int BoxId { get; set; }
        public Int64 Quantity { get; set; }
        public string? Description { get; set; }
        public StatusEnum? IsChecked { get; set; } 
    }
}

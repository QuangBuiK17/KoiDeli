﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxOptionDTOs
{
    public class BoxOptionDTO
    {
        public int FishId { get; set; }
        public int BoxId { get; set; }
        public string? Description { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}

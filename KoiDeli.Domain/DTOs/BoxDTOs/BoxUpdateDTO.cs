﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxDTOs
{
    public class BoxUpdateDTO
    {
        public string? Name { get; set; }
        public Int64 MaxVolume { get; set; }
        public Int64 Price { get; set; }
    }
}

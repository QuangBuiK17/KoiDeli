﻿using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderDTOs
{
    public class OrderUpdateDTO
    {
        public int DistanceId { get; set; }
        //receive 
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }
        public StatusEnum? IsShipping { get; set; }

        // File URL
        public string? URLCer1 { get; set; }
        public string? URLCer2 { get; set; }
        public string? URLCer3 { get; set; }
        public string? URLCer4 { get; set; }

    }
}

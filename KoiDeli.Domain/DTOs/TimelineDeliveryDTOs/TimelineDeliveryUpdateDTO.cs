﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.TimelineDeliveryDTOs
{
    public class TimelineDeliveryUpdateDTO
    {
        public int VehicleId { get; set; }
        public int BranchId { get; set; }

        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? TimeCompleted { get; set; }
        public string? Description { get; set; }
    }
}

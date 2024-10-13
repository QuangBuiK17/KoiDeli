using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.TimelineDeliveryDTOs
{
    public class TimelineDeliveryDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int BranchId { get; set; }

        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public StatusEnum? IsCompleted { get; set; } 
        public DateTime? TimeCompleted { get; set; }
        public string? Description { get; set; }
    }
}

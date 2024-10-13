using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderTimelineDTOs
{
    public class OrderTimelineDTO
    {
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public int TimelineDeliveryId { get; set; }

        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public StatusEnum? IsCompleted { get; set; } 
        public DateTime? TimeCompleted { get; set; }
        public string? Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderTimelineDTOs
{
    public class OrderTimelineUpdateDTO
    {
        public int OrderDetailId { get; set; }
        public int TimelineDeliveryId { get; set; }

        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? TimeCompleted { get; set; }
        public string? Description { get; set; }
    }
}

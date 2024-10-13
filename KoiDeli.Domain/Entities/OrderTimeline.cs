using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class OrderTimeline: BaseEntity
    {
        public int OrderDetailId { get; set; }
        public int TimelineDeliveryId { get; set; }

        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public string? IsCompleted { get; set; } 
        public DateTime? TimeCompleted { get; set; }
        public string? Description { get; set; }

        //relation
        public virtual OrderDetail? OrderDetail { get; set; }
        public virtual TimelineDelivery? TimelineDelivery { get; set; }
    }
}

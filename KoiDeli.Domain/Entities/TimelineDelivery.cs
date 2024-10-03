using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class TimelineDelivery: BaseEntity
    {
        public int VehicleId { get; set; }
        public int BranchId { get; set; }

        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public bool IsCompleted { get; set; }=false;
        public DateTime? Completed { get; set; }
        public string? Description { get; set; }

        //Relation

        public virtual Vehicle? Vehicle { get; set; }
        public virtual Branch? Branch { get; set; }        
        public virtual ICollection<OrderTimeline>? OrderTimelines { get; set; }

    }
}

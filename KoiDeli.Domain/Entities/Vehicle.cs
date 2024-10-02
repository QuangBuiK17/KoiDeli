using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Vehicle: BaseEntity
    {
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Int64 VehicleVolume { get; set; }

        //Relation

        public virtual ICollection<TimelineDelivery>? TimelineDeliveries { get; set; }


    }
}

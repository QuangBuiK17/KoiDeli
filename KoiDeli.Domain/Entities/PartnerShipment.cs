using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class PartnerShipment: BaseEntity
    {
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? Completed { get; set; }
        public string? Description { get; set; } = null;
        public Int64 Price { get; set; }

        //relation
        public virtual OrderDetail? OrderDetail { get; set; }
    }
}

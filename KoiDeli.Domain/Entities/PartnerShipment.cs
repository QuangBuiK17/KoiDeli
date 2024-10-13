using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class PartnerShipment: BaseEntity
    {
        public string? Name { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public string? IsCompleted { get; set; } 
        public DateTime? Completed { get; set; }
        public string? Description { get; set; } = null;
        public Int64 Price { get; set; }

        //relation
        public virtual ICollection<OrderDetail>?  OrderDetail { get; set; }
    }
}

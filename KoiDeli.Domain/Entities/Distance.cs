using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Distance: BaseEntity
    {
        public Int64 RangeDistance { get; set; }
        public Int64 Price {get; set; }

        //relation
        public virtual ICollection<Order>? Order { get; set; }

    }
}

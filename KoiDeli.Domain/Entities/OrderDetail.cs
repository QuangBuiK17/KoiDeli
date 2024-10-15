using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class OrderDetail: BaseEntity
    {
        public Int64 TotalShippingFee { get;set;}
        public int BoxOptionId { get; set;}
        public int OrderId { get; set;}
        public int DistanceId { get; set;}
        public string? IsComplete { get; set;} 

        //relation
        public virtual ICollection<OrderTimeline>? OrderTimelines { get; set;}
        public virtual BoxOption? BoxOption { get; set;}
        public virtual Order? Order { get; set;}
        public virtual Distance? Distance { get; set;}

    }
}

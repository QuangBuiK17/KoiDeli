using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class OrderDetail: BaseEntity
    {
        public Int64 LocalShipingFee { get; set; }
        public Int64 PartnerShippingFee { get; set;}
        public Int64 TotalShippingFee { get;set;}
        public int BoxID { get; set;}
        public int FishID { get; set;}
        public int OrderID { get; set;}
        public int DistanceID { get; set;}
        public bool IsComplete { get; set;} = false;

        //relation
        public virtual PartnerShipment? PartnerShipment { get; set;}
        public virtual ICollection<OrderTimeline>? OrderTimelines { get; set;}

    }
}

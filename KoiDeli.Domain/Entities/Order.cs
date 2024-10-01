using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Order: BaseEntity
    {
        public int CustomerID { get; set; }
        public int Payment { get; set; }
        public int FeedbackID { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }= string.Empty;
        public string? ReceiverPhone { get; set; }
        public Int64 TotalShipment {  get; set; }
        public bool IsShipping { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? ShippingTime { get; set; }

        public DateTime? ShippingStart { get;set; }
        public DateTime? ShippingEnd { get; set; }



    }
}

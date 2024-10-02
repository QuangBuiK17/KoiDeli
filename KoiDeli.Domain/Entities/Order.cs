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
        public int PaymentID { get; set; }
        public int FeedbackID { get; set; }
        //receive 
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }= string.Empty;
        public string? ReceiverPhone { get; set; }
        //cost
        public Int64 TotalShipment {  get; set; }
        public Int64 TotalBox {  get; set; }
        public Int64 TotalPackingFee { get; set; }
        public Int64 TotalShippingFee { get; set; }
        // time shiping
        public bool IsShipping { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? ShippingTime { get; set; }
        public DateTime? ShippingStart { get;set; }
        public DateTime? ShippingEnd { get; set; }

        //Relation
        public virtual Payment? Payment { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Feedback>? Feedbacks {  get; set; } 

    }
}

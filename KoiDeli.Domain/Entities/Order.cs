using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Order: BaseEntity
    {
        public int FeedbackId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderAddress { get; set; }
        //receive 
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }
        //cost
        public Int64 TotalBox { get; set; }
        public Int64 TotalFee { get; set; }
        // time shiping
        public string? IsShipping { get; set; }
        // File URL
        public string? URLCer1 { get; set; }
        public string? URLCer2 { get; set; }
        public string? URLCer3 { get; set; }
        public string? URLCer4 { get; set; }



        //Relation
        public virtual User? User { get; set; }
        public virtual ICollection<Feedback>? Feedbacks {  get; set; } 
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        

    }
}

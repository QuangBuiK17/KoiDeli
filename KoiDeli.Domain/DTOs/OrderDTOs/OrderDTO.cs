using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? SenderName { get; set; }
        public string? SenderAddress { get; set; }
        public bool isDeleted { get; set; }
        public int FeedbackId { get; set; }
        //receive 
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }
        //cost
        public Int64 TotalBox { get; set; }
        public Int64 TotalFee { get; set; }
        // time shiping
        public StatusEnum? IsShipping { get; set; }

        // File URL
        public string? URLCer1 { get; set; }
        public string? URLCer2 { get; set; }
        public string? URLCer3 { get; set; }
        public string? URLCer4 { get; set; }

    }
}

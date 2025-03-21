﻿using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderDTOs
{
    public class OrderCreateDTO
    {
        public int UserId { get; set; }
        //receive 
        public string? SenderName { get; set; }
        public string? SenderAddress { get; set; }
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }
        public Int64 TotalFee { get; set; }
        public StatusEnum? IsShipping { get; set; }
        public bool IsPayment { get; set; }
        // File URL
        public string? URLCer1 { get; set; }
        public string? URLCer2 { get; set; }
        public string? URLCer3 { get; set; }
        public string? URLCer4 { get; set; }

    }
}

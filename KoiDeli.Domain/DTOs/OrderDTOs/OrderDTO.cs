﻿using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int FeedbackId { get; set; }
        public int DistanceId { get; set; }
        //receive 
        public string? ReceiverName { get; set; }
        public string? ReceiverAddress { get; set; }
        public string? ReceiverPhone { get; set; }
        //cost
        public Int64 TotalShipment { get; set; }
        public Int64 TotalBox { get; set; }
        public Int64 TotalPackingFee { get; set; }
        public Int64 TotalShippingFee { get; set; }
        // time shiping
        public StatusEnum? IsShipping { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? ShippingTime { get; set; }
        public DateTime? ShippingStart { get; set; }
        public DateTime? ShippingEnd { get; set; }

        // File URL
        public string? URLCer1 { get; set; }
        public string? URLCer2 { get; set; }
        public string? URLCer3 { get; set; }
        public string? URLCer4 { get; set; }

    }
}

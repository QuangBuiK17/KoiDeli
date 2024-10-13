using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderDetailDTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public Int64 LocalShipingFee { get; set; }
        public Int64 PartnerShippingFee { get; set; }
        public Int64 TotalShippingFee { get; set; }
        public int ParnerShipmentId { get; set; }
        public int BoxOptionId { get; set; }
        public int FishId { get; set; }
        public int OrderId { get; set; }
        public StatusEnum IsComplete { get; set; } 
    }
}

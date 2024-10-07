using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderDetailDTOs
{
    public class OrderDetailCreateDTO
    {
        public Int64 LocalShipingFee { get; set; }
        public Int64 PartnerShippingFee { get; set; }
        public Int64 TotalShippingFee { get; set; }
        public int ParnerShipmentId { get; set; }
        public int BoxOptionId { get; set; }
        public int OrderId { get; set; }
        public bool IsComplete { get; set; } = false;
    }
}

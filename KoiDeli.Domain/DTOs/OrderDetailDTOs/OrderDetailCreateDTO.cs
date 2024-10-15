using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.OrderDetailDTOs
{
    public class OrderDetailCreateDTO
    {
        public Int64 TotalShippingFee { get; set; }
        public int BoxOptionId { get; set; }
        public int OrderId { get; set; }
        public int DistanceId { get; set; }
        public StatusEnum? IsComplete { get; set; }
    }
}

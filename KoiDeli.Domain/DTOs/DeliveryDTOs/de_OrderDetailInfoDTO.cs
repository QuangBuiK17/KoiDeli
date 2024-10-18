using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DeliveryDTOs
{
    public class de_OrderDetailInfoDTO
    {
        public int OrderDetailID { get; set; }
        public int BoxOptionID { get; set; }
        public string? BoxName { get; set; }
        public long BoxVolume { get; set; }
        //public Int64 Quantity { get; set; }
        //public long TotalVolume => BoxVolume * Quantity;
    }
}

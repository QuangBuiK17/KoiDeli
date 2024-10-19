using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DeliveryDTOs
{
    public class de_ViewOrderDetailInTimelineDTO
    {
        public int TimelineID { get; set; }
        public string IsComplete { get; set; }
        public string VehicleName { get; set; }
        public string BranchName { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public long Maxvolume { get; set; }
        public long RemainingVolume { get; set; }
        public List<de_OrderDetailDTO> OrderDetails { get; set; }
    }

    public class de_OrderDetailDTO
    {
        public int DetailID { get; set; }
        public string IsComplete { get; set; }
        public string BoxName { get; set; }
        public double Volume { get; set; }
    }
}

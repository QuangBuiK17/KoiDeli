using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DeliveryDTOs
{
    public class de_ViewScheduleOfOrdetailDTO
    {
        public int DetailID { get; set; }
        public string IsComplete { get; set; }
        public int BoxOptionID { get; set; }
        public string BoxName { get; set; }
        public double Volume { get; set; }
        public List<de_OrderTimelineDTO> OrderTimelines { get; set; }
    }

    public class de_OrderTimelineDTO
    {
        public int OrderTimelineID { get; set; }
        public string IsComplete { get; set; }
        public int TimelineID { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public string Start_From { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DeliveryDTOs
{
    public class de_SuitableTimelineDTO
    {
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
        public Int64 VehicleVolume { get; set; }
        public List<ListTimeline> Timelines { get; set; }
    }

    public class ListTimeline
    {
        public int TimelineId { get; set; }
        public string isComplete { get; set; }
        public string BranchName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int64 CurrentVolume { get; set; }
        public Int64 RemainingVolume { get; set; }
    }

    public class FillterSuitableTimeline
    {
        public int orderDetailID { get; set; }
        public List<int> branch { get; set; }
        public DateTime? startDay { get; set; }
    }
}

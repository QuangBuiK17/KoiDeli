using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DeliveryDTOs
{
    public class de_TimelineDeliveryInfoDTO
    {
        public int TimelineDeliveryID { get; set; }
        public string isComplete { get; set; }
        public int BranchID { get; set; }
        public string? StartPoint { get; set; }
        public string? EndPoint { get; set; }
        public string StartDay { get; set; } // Format: YYYY-MM-DD
        public string EndDay { get; set; }   // Format: YYYY-MM-DD
        public string StartTime { get; set; } // Format: HH-MM-SS
        public string EndTime { get; set; }   // Format: HH-MM-SS
        public int VehicleID { get; set; }
        public string? VehicleName { get; set; }
        public long MaxVolume { get; set; }
        public long CurentVolume { get; set; }
        //public long RemainingVolume { get; set; }

        public long RemainingVolume 
        {
            get
            {
                return (long) (MaxVolume - CurentVolume);
            }
        }
    }

}

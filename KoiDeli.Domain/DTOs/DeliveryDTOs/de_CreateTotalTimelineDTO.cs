using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DeliveryDTOs
{
    public class de_CreateTotalTimelineDTO
    {
        public string TotalStartPoint { get; set; }
        public string TotalEndPoint { get; set; }
        public int VehicleID { get; set; }
        public List<de_CreateDetailTimelineDTO> de_CreateDetailTimelineDTOs { get; set; }
    }



    public class de_CreateDetailTimelineDTO
    {
        public int BranchId { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
    }
}
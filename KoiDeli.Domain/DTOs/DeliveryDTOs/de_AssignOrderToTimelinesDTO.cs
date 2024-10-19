using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DeliveryDTOs
{
    public class de_AssignOrderToTimelinesDTO
    {
       public int OrderDetailID { get; set; }
       public List<int> TimelineID { get; set; }
    }
}

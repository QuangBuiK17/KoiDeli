using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.PartnerShipmentDTOs
{
    public class PartnerUpdateCompleteDTO
    {
        public bool IsCompleted { get; set; } 
        public DateTime? Completed { get; set; }
    }
}

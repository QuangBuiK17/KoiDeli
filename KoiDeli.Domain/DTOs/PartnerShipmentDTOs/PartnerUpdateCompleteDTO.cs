using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.PartnerShipmentDTOs
{
    public class PartnerUpdateCompleteDTO
    {
        public StatusEnum? IsCompleted { get; set; } 
        public DateTime? Completed { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.PartnerShipmentDTOs
{
    public class PartnerDTO
    {
        public int id {  get; set; }
        public bool isDeleted { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? Completed { get; set; }
        public string? Description { get; set; } = null;
        public Int64 Price { get; set; }
    }
}

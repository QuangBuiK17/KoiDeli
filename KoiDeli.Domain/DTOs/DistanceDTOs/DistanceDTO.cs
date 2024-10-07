using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.DistanceDTOs
{
    public class DistanceDTO
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public Int64 RangeDistance { get; set; }
        public Int64 Price { get; set; }
    }
}

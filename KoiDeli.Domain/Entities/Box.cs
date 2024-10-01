using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Box: BaseEntity
    {
        public string? BoxType { get; set; }
        public Int64 BoxCapacity { get; set; }
        public Int64 BoxVolume { get; set; }
        public Int64 BoxPrice { get; set; }


    }
}

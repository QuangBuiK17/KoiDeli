using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Box: BaseEntity
    {
        public Type? BoxType { get; set; }
        public Int64 BoxCapacity { get; set; }
        public Int64 BoxWeight { get; set; }
        public Int64 BoxPrice { get; set; }


    }
}

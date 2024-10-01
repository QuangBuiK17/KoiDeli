using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class KoiFish: BaseEntity
    {
        public Int64 Size { get; set; }
        public Int64 FishWeight { get; set; }
        public string? Description { get; set; }

    }
}

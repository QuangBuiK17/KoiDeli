using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class FishInBox : BaseEntity
    {
        public int FishId { get; set; }
        public int BoxOptionId { get; set; }
        public Int64 Quantity { get; set; }

        public virtual KoiFish? KoiFish { get; set; }
        public virtual BoxOption? BoxOption { get; set; }
    }
}

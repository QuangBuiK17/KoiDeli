using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class BoxOption: BaseEntity
    {
        public int FishID { get; set; }
        public int BoxID { get; set; }
        public string? Description { get; set; }
        public bool IsChecked { get; set; } = false;

        //relation
        public virtual Box? Box { get; set; }
        public virtual KoiFish? Fish { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        
    }
}

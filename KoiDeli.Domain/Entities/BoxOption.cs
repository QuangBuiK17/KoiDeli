using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class BoxOption: BaseEntity
    {
        public int FishId { get; set; }
        public int BoxId { get; set; }
        public string? Description { get; set; }
        public string? IsChecked { get; set; } 

        //relation
        public virtual Box? Box { get; set; }
        public virtual KoiFish? Fish { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        
    }
}

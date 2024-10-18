using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class BoxOption: BaseEntity
    {
        public int BoxId { get; set; }

        // Một BoxOption có thể chứa nhiều loại cá (FishInBox)
        public string? Description { get; set; }
        public string? IsChecked { get; set; }

        //relation
        public virtual Box? Box { get; set; }
        public virtual OrderDetail? OrderDetail { get; set; }
        public virtual ICollection<FishInBox>? FishInBoxes { get; set; }
    }
}

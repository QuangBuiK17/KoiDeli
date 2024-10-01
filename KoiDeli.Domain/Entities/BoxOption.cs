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
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class KoiFish: BaseEntity
    {
        public string Size { get; set; }
        public Int64 Volume { get; set; }
        public string? Description { get; set; }

        //Relation
        public virtual ICollection<BoxOption>? BoxOptions { get; set; }


    }
}

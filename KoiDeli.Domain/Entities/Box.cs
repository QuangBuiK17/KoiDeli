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
        public Int64 MaxVolume { get; set; }
        public Int64 Price { get; set; }

        //Relation
        public virtual ICollection<BoxOption>? BoxOptions { get; set; }


    }
}

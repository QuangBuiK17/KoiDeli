using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Box: BaseEntity
    {
        public string? Name { get; set; }
        public Int64 MaxVolume { get; set; }
        public Int64 Price { get; set; }
        [NotMapped] public Int64 RemainingVolume { get; set; }

        //Relation
        public virtual ICollection<BoxOption>? BoxOptions { get; set; }

    }
}

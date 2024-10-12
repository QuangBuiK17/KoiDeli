using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class KoiFish: BaseEntity
    {
        [Column(TypeName = "nvarchar(MAX)")]
        public string? Size { get; set; }
        public Int64 Volume { get; set; }
        public string? Description { get; set; }

        //Relation
        public virtual ICollection<BoxOption>? BoxOptions { get; set; }


    }
}

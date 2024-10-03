using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Feedback: BaseEntity
    {
        public string? Desciption { get; set; }

        //Relation
        public virtual Order? Order { get; set; }

    }
}

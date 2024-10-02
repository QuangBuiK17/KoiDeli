using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Role
    {
        public string? Name { get; set; }

        //Relation
        public virtual ICollection<User>? Users { get; set; }
    }
}

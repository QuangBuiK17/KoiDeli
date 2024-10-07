using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BranchDTOs
{
    public class BranchUpdateDTO
    {
        public string? Name { get; set; }
        public string? StartPoint { get; set; }
        public string? EndPoint { get; set; }
    }
}

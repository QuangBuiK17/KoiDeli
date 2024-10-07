using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BranchDTOs
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? StartPoint { get; set; }
        public string? EndPoint { get; set; }
    }
}

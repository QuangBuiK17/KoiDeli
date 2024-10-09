using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.FeedbackDTOs
{
    public class FeedbackDTO
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string? Desciption { get; set; }
        public int OrderId { get; set; }
    }
}

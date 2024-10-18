using KoiDeli.Domain.DTOs.FishInBoxDTOs;
using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxOptionDTOs
{
    public class BoxOptionDTO
    {
        public int Id { get; set; }
        public int BoxId { get; set; }
        public string? Description { get; set; }

        // Danh sách các loại cá trong hộp (FishInBoxDTO)
        public List<FishInBoxDTO>? FishInBoxes { get; set; }
        public StatusEnum? IsChecked { get; set; } 
    }
}

using KoiDeli.Domain.DTOs.FishInBoxDTOs;
using KoiDeli.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxOptionDTOs
{
    public class BoxOptionCreateDTO
    {
        public int BoxId { get; set; }
        public List<FishInBoxDTO>? FishInBoxes { get; set; }  // Chứa danh sách FishInBox
        public StatusEnum? IsChecked { get; set; }
    }
}

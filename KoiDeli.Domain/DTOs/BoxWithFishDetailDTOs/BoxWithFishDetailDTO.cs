using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxWithFishDetailDTOs
{
    public class BoxWithFishDetailDTO
    {
        public Box? Box { get; set; }
        public List<KoiFish> Fishes { get; set; } = new List<KoiFish>();
        public int TotalFish => Fishes.Count;
    }
}

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
        public BoxModelOptimize? Box { get; set; }
        public List<KoiFishModelOptimize> Fishes { get; set; } = new List<KoiFishModelOptimize>();
        public int TotalFish => Fishes.Count;
    }
}

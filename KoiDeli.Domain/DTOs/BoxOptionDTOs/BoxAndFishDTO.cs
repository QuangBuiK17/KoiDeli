using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxOptionDTOs
{
    public class BoxOpDTO
    {
        public string? BoxName { get; set; }
        public int BoxId { get; set; }
        public Int64 MaxVolume { get; set; }
        public Int64 RemainingVolume { get; set; }
        public int UsageCount { get; set; }
        public int TotalFish { get; set; }
        public Int64? TotalVolume { get; set; }
        public Int64 Price { get; set; }
        public List<FishDTO> Fishes { get; set; } = new List<FishDTO>();
    }

    public class FishDTO
    {
        public int FishId { get; set; }
        public string? FishSize { get; set; }
        public Int64 FishVolume { get; set; }
        public string? FishDescription { get; set; }
        public Int64 Quantity { get; set; }
    }

}

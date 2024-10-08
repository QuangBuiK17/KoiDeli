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

        public Int64 BoxPrice { get; set; }  // Price of the individual box

        public Int64 TotalPrice // Calculate the total price for the box, including any other relevant calculations
        {
            get
            {
                // Here, you can adjust this to factor in other costs or discounts if needed
                return BoxPrice;
            }
        }
    }
}

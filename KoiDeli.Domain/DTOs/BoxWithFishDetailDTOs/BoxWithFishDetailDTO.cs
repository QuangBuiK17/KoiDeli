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

        // Tính tổng số lượng cá dựa trên Quantity
        public Int64 TotalFish
        {
            get
            {
                return Fishes.Sum(fish => fish.Quantity);  // Tính tổng Quantity
            }
        }

        public Int64 BoxPrice { get; set; }  // Price of the individual box

        public Int64 TotalPrice // Calculate the total price for the box, including any other relevant calculations
        {
            get
            {
                // Here, you can adjust this to factor in other costs or discounts if needed
                return BoxPrice;
            }
        }

        public Int64? TotalVolume { get; set; }
    }
}

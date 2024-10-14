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

        public int UsageCount { get; set; } // Thêm thuộc tính này để đếm số lần tái sử dụng của hộp

        public Int64 TotalFish
        {
            get
            {
                return Fishes.Sum(fish => fish.Quantity);  // Tính tổng Quantity
            }
        }

        public Int64 BoxPrice { get; set; }

        public Int64 TotalPrice
        {
            get
            {
                return BoxPrice;
            }
        }

        public Int64? TotalVolume { get; set; }
    }

}

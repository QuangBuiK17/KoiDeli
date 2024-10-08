using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxWithFishDetailDTOs
{
    public class BoxModelOptimize
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Int64 MaxVolume { get; set; }
        public Int64 Price { get; set; }
        [NotMapped] public Int64 RemainingVolume { get; set; }


        // xu li logic xep ca
        public BoxModelOptimize()
        {
            RemainingVolume = MaxVolume; // Khi khởi tạo hộp, thể tích còn lại bằng MaxVolume
        }

        // Kiểm tra xem hộp có thể chứa thêm cá dựa trên thể tích cá và thể tích còn lại của hộp
        public bool CanFitFish(KoiFishModelOptimize fish)
        {
            return RemainingVolume >= fish.Volume;
        }

        // Thêm cá vào hộp và cập nhật thể tích còn lại
        public int AddFish(KoiFishModelOptimize fish)
        {
            while (fish.Quantity > 0 && CanFitFish(fish))
            {
                RemainingVolume -= fish.Volume;
                fish.Quantity--;  // Giảm số lượng cá sau mỗi lần thêm vào
            }

            return (int)fish.Quantity; // Trả về số lượng cá chưa được đóng gói
        }

    }
}

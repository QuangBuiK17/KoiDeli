using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Box: BaseEntity
    {
        public string? Name { get; set; }
        public Int64 MaxVolume { get; set; }
        public Int64 Price { get; set; }
        [NotMapped] public Int64 RemainingVolume { get; set; }

        //Relation
        public virtual ICollection<BoxOption>? BoxOptions { get; set; }
        // xu li logic xep ca
        public Box()
        {
            RemainingVolume = MaxVolume; // Khi khởi tạo hộp, thể tích còn lại bằng MaxVolume
        }

        // Kiểm tra xem hộp có thể chứa thêm cá dựa trên thể tích cá và thể tích còn lại của hộp
        public bool CanFitFish(KoiFish fish)
        {
            return RemainingVolume >= fish.Volume;
        }

        // Thêm cá vào hộp và cập nhật thể tích còn lại
        public void AddFish(KoiFish fish)
        {
            if (CanFitFish(fish))
            {
                RemainingVolume -= fish.Volume;
            }
            else
            {
                throw new InvalidOperationException("Không thể thêm cá vào hộp vì không đủ thể tích.");
            }
        }
    }
}

using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxWithFishDetailDTOs
{
    public class PackingRequestDTO
    {
        public List<KoiFish> FishList { get; set; }
        public List<Box> BoxList { get; set; }
    }
}

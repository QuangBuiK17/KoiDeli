using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.BoxOptionDTOs
{
    public class BoxOptionCreateRequest
    {
        public List<BoxData> Boxes { get; set; }

        public class BoxData
        {
            public int BoxId { get; set; }
            public List<FishData>? Fishes { get; set; }
        }

        public class FishData
        {
            public int FishId { get; set; }
            public Int64 Quantity { get; set; }
        }
    }
}

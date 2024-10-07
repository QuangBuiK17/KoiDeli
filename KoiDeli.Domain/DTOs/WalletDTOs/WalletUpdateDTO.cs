using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.WalletDTOs
{
    public class WalletUpdateDTO
    {
        public int UserId { get; set; }
        public string? WalletType { get; set; }
        public long Balance { get; set; }
    }
}

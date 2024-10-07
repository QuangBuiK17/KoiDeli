using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.TransactionDTOs
{
    public class TransactionCreateDTO
    {
        public Int64 TotalAmount { get; set; }
        public string? PaymentType { get; set; }
        public int WalletId { get; set; }
    }
}

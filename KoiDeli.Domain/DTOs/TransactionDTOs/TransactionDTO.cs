using KoiDeli.Domain.DTOs.WalletDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.TransactionDTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 TotalAmount { get; set; }
        public string? PaymentType { get; set; }
        public int WalletId { get; set; }
    }
}

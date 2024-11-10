using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Int64 TotalAmount { get; set; }
        public string? PaymentType { get; set; }
        public string? TransactionStatus { get; set; } // Status of transaction (Pending, Success, Failed)
        public DateTime? TransactionDate { get; set; } // Date of transaction

        // Relation to Wallet
        public Wallet? Wallet { get; set; }
    }

}

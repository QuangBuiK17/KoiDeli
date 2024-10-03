using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.Entities
{
    public class Transaction: BaseEntity
    {
        public Int64 TotalAmount { get; set; }
        public Type? PaymentType { get; set; }

        //Relation

        public Wallet? Wallet { get; set; }
        public virtual ICollection<TransactionDetail>? TransactionDetails { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KoiDeli.Domain.Entities
{
    public class Wallet: BaseEntity
    {
        public int UserId { get; set; }
        public string? WalletType { get; set; }
        public long Balance { get; set; }

        //relations

        public virtual User? User { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}

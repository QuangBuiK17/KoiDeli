using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace KoiDeli.Domain.Entities
{
    public class TransactionDetail: BaseEntity
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }

        //realation
        public virtual Transaction? Payment { get; set; }
        public virtual Order? Order { get; set; }
    }
}

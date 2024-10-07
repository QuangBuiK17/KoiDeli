using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<bool> CheckWalletIdExisted(int id);
        Task<List<TransactionDTO>> GetTransactionsEnabledAsync();
        Task<Wallet> CheckWalletExisted(int id);
        Task<int> GetWalletIdByTransactionIdAsync(int transactionId);
        Task<List<TransactionDTO>> GetTransactionInfoAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);

    }
}

using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly KoiDeliDbContext _context;
        public TransactionRepository(KoiDeliDbContext context,
                                     ICurrentTime timeService,
                                     IClaimsService claimsService) 
            : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<Wallet> CheckWalletExisted(int id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<bool> CheckWalletIdExisted(int id)
        {
            return await _context.Wallets.AnyAsync(w => w.Id == id);
        }

        public async Task<List<TransactionDTO>> GetTransactionsEnabledAsync()
        {
            var transactionInfoList = await _context.Transaction
              .Where(transaction => transaction.IsDeleted == false) 
              .Select(transaction => new TransactionDTO
              {
                  Id = transaction.Id,
                  IsDeleted = transaction.IsDeleted,
                  TotalAmount = transaction.TotalAmount,
                  PaymentType = transaction.PaymentType,
                  WalletId = transaction.Wallet.Id
              })
              .ToListAsync();

            return transactionInfoList;
        }

        public async Task<int> GetWalletIdByTransactionIdAsync(int transactionId)
        {
            var transaction = await _context.Transaction
            .Where(t => t.Id == transactionId && !t.IsDeleted) // Điều kiện kiểm tra xem transaction có bị xóa không
            .FirstOrDefaultAsync();
            if (transaction == null || transaction.Wallet.Id == null)
            {
                return 0;
            }
            return transaction.Wallet.Id;

        }

        public async Task<List<TransactionDTO>> GetTransactionInfoAsync()
        {
            var transactionInfoList = await _context.Transaction
                .Select(transaction => new TransactionDTO
                {
                    Id = transaction.Id,
                    IsDeleted = transaction.IsDeleted,
                    TotalAmount = transaction.TotalAmount, 
                    PaymentType = transaction.PaymentType,
                    WalletId = transaction.Wallet.Id 
                })
                .ToListAsync();

            return transactionInfoList;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            var transaction = await _context.Transaction
        .Where(t => t.Id == id && t.IsDeleted == false) 
        .Select(t => new Transaction
        {
            Id = t.Id,
            IsDeleted = t.IsDeleted,
            TotalAmount = t.TotalAmount,
            PaymentType = t.PaymentType,
            Wallet = new Wallet 
            {
                Id = t.Wallet.Id, 
             }
        })
        .FirstOrDefaultAsync(); // Trả về transaction đầu tiên (hoặc null nếu không có)

            return transaction;
        }
    }
}

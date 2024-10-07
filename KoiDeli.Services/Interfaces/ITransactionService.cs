using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResult<List<TransactionDTO>>> GetAsync();
        Task<ApiResult<List<TransactionDTO>>> GetEnabledAsync();
        Task<ApiResult<TransactionDTO>> GetByIdAsync(int id);
        Task<ApiResult<TransactionDTO>> DeleteAsync(int id);
        Task<ApiResult<TransactionDTO>> UpdatetAsync(int id, TransactionUpdateDTO updateDto);
        Task<ApiResult<TransactionDTO>> CreateAsync(TransactionCreateDTO creaeDTO);
    }
}

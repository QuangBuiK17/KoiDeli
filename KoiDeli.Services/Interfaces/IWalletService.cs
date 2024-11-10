using KoiDeli.Domain.DTOs.VnPayDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IWalletService
    {
        Task<ApiResult<List<WalletDTO>>> GetWalletAsync();
        Task<ApiResult<List<WalletDTO>>> GetWalletEnabledAsync();
        Task<ApiResult<WalletDTO>> GetWalletByIdAsync(int id);
        Task<ApiResult<WalletDTO>> DeleteWalletAsync(int id);
        Task<ApiResult<WalletDTO>> UpdateWalletAsync(int id,WalletUpdateDTO updateDto);
        Task<ApiResult<WalletDTO>> CreateWalletAsync(WalletCreateDTO creaeDTO);
       // Task<ApiResult<DepositResponseDTO>> Deposit(long amount, int id);
    }
}

using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Repositories.Common;


namespace KoiDeli.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<ApiResult<AccountDTO>> RegisterAsync(RegisterAccountDTO registerAccountDTO);
        public Task<ApiResult<string>> LoginAsync(AuthenAccountDTO accountDto);
        public Task<ApiResult<string>> ForgotPassword(string email);
    }
}

using KoiDeli.Domain.DTOs.FeedbackDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IFeedbackService
    {
        Task<ApiResult<List<FeedbackDTO>>> GetFeedbackAsync();
        Task<ApiResult<FeedbackDTO>> GetFeedbackByIdAsync(int id);
        Task<ApiResult<FeedbackDTO>> DeleteFeedbackAsync(int id);
        Task<ApiResult<FeedbackDTO>> UpdateFeedbackAsync(int id, FeedbackUpdateDTO updateDto);
        Task<ApiResult<FeedbackDTO>> CreateFeedbackAsync(FeedbackCreateDTO creaeDTO);
        Task<ApiResult<List<FeedbackDTO>>> GetFeedbacksEnabledAsync();
    }
}

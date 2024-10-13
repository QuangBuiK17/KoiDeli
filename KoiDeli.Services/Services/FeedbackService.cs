using AutoMapper;
using KoiDeli.Domain.DTOs.FeedbackDTOs;
using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResult<FeedbackDTO>> CreateFeedbackAsync(FeedbackCreateDTO creaeDTO)
        {
            var response = new ApiResult<FeedbackDTO>();
            try
            {
                

                var order = await _unitOfWork.FeedbackRepository.ChecOrderExisted (creaeDTO.OrderId);
                if (order == null)
                {
                    response.Success = false;
                    response.Message = "OrderID is invalid";
                    return response;
                }
                var feedback = new Feedback
                {
                    Desciption = creaeDTO.Desciption,
                    Order = order
                };
                await _unitOfWork.FeedbackRepository.AddAsync(feedback);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<FeedbackDTO>(feedback);
                    response.Success = true;
                    response.Message = "Create new feedback successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Create new feedback fail";
                    return response;
                }
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }

        public async Task<ApiResult<FeedbackDTO>> DeleteFeedbackAsync(int id)
        {
            var response = new ApiResult<FeedbackDTO>();
            var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                response.Success = false;
                response.Message = "feedback not found!";
                return response;
            }
            else
            {
                _unitOfWork.FeedbackRepository.SoftRemove(feedback);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<FeedbackDTO>(feedback);
                    response.Success = true;
                    response.Message = "Delete feedback successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Delete feedback fail";
                    return response;
                }
            }
        }

        public async Task<ApiResult<List<FeedbackDTO>>> GetFeedbackAsync()
        {
            var response = new ApiResult<List<FeedbackDTO>>();
            List<FeedbackDTO> FeedbackDTOs = new List<FeedbackDTO>();
            try
            {
                var feedbacks = await _unitOfWork.FeedbackRepository.GetFeedbackInfoAsync();
                if (feedbacks.Count > 0)
                {
                    response.Data = feedbacks;
                    response.Success = true;
                    response.Message = $"Found {feedbacks.Count} feedbacks.";
                    response.Error = "No error";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No feedbacks found.";
                    response.Error = "No error";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = "Exception";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ApiResult<FeedbackDTO>> GetFeedbackByIdAsync(int id)
        {
            var response = new ApiResult<FeedbackDTO>();
            try
            {
                var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackByIdAsync(id);

                if (feedback == null || feedback.IsDeleted)
                {
                    response.Success = false;
                    response.Message = "Feedback ID doesn't exist or has been deleted!";
                    return response;
                }
                var feedbackDto = _mapper.Map<FeedbackDTO>(feedback);
                feedbackDto.OrderId = feedback.Order.Id; 

                // Map transaction sang DTO và trả về kết quả
                response.Data = feedbackDto;
                response.Success = true;
                response.Message = "Feedback ID is valid";
            }
            catch (Exception ex)
            {
                
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<FeedbackDTO>>> GetFeedbacksEnabledAsync()
        {
            var response = new ApiResult<List<FeedbackDTO>>();
            List<FeedbackDTO> FeedbackDTOs = new List<FeedbackDTO>();
            try
            {
                var feedbacks = await _unitOfWork.FeedbackRepository.GetFeedbacksEnabledAsync();
                if (feedbacks.Count > 0)
                {
                    response.Data = feedbacks;
                    response.Success = true;
                    response.Message = $"Found {feedbacks.Count} feedbacks enable.";
                    response.Error = "No error";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No feedbacks enable found.";
                    response.Error = "No error";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = "Exception";
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ApiResult<FeedbackDTO>> UpdateFeedbackAsync(int id, FeedbackUpdateDTO updateDto)
        {
            var response = new ApiResult<FeedbackDTO>();

            try
            {
                var enityById = await _unitOfWork.FeedbackRepository.GetByIdAsync(id);

                if (enityById != null)
                {

                    var newb = _mapper.Map(updateDto, enityById);
                    var bAfter = _mapper.Map<Feedback>(newb);
                    _unitOfWork.FeedbackRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        //response.Data = _mapper.Map<FeedbackDTO>(bAfter);
                        var feedback = await _unitOfWork.FeedbackRepository.GetFeedbackByIdAsync(id);
                        var feedbackDto = _mapper.Map<FeedbackDTO>(feedback);
                        feedbackDto.OrderId = feedback.Order.Id;
                        response.Data = feedbackDto;
                        response.Message = $"Successfull for update feedback.";
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Error = "Save update failed";
                        return response;
                    }

                }
                else
                {
                    response.Success = false;
                    response.Message = $"Have no feedback.";
                    response.Error = "Not have a feedback";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }
    }
}

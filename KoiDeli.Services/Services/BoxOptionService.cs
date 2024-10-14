using AutoMapper;
using KoiDeli.Domain.DTOs.BoxOptionDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Domain.Enums;
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
    public class BoxOptionService : IBoxOptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public BoxOptionService(
            IUnitOfWork unitOfWork,
            ICurrentTime currentTime,
            AppConfiguration configuration,
            IMapper mapper
        )
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ApiResult<BoxOptionDTO>> CreateBoxOptionAsync(BoxOptionCreateDTO boxOptionDto)
        {
            var response = new ApiResult<BoxOptionDTO>();

            try
            {
                var entity = _mapper.Map<BoxOption>(boxOptionDto);

                entity.IsChecked = boxOptionDto.IsChecked.HasValue
                    ? boxOptionDto.IsChecked.Value.ToString()
                    : StatusEnum.Pending.ToString();

                await _unitOfWork.BoxOptionRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<BoxOptionDTO>(entity);
                    response.Success = true;
                    response.Message = "BoxOption created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create BoxOption.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<BoxOptionDTO>> DeleteBoxOptionAsync(int id)
        {
            var response = new ApiResult<BoxOptionDTO>();
            var boxOption = await _unitOfWork.BoxOptionRepository.GetByIdAsync(id);

            if (boxOption != null)
            {
                _unitOfWork.BoxOptionRepository.SoftRemove(boxOption);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<BoxOptionDTO>(boxOption);
                    response.Success = true;
                    response.Message = "BoxOption deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete BoxOption.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "BoxOption not found.";
            }

            return response;
        }

        public async Task<ApiResult<BoxOptionDTO>> GetBoxOptionByIdAsync(int id)
        {
            var response = new ApiResult<BoxOptionDTO>();

            try
            {
                var boxOption = await _unitOfWork.BoxOptionRepository.GetByIdAsync(id);

                if (boxOption != null)
                {
                    response.Data = _mapper.Map<BoxOptionDTO>(boxOption);
                    response.Success = true;
                    response.Message = "BoxOption retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "BoxOption not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<BoxOptionDTO>>> GetBoxOptionsAsync()
        {
            var response = new ApiResult<List<BoxOptionDTO>>();
            List<BoxOptionDTO> boxOptionDTOs = new List<BoxOptionDTO>();

            try
            {
                var boxOptions = await _unitOfWork.BoxOptionRepository.GetAllAsync();

                foreach (var boxOption in boxOptions)
                {
                    var boxOptionDto = _mapper.Map<BoxOptionDTO>(boxOption);
                    boxOptionDTOs.Add(boxOptionDto);
                }

                if (boxOptionDTOs.Count > 0)
                {
                    response.Data = boxOptionDTOs;
                    response.Success = true;
                    response.Message = $"Found {boxOptionDTOs.Count} BoxOptions.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No BoxOptions found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public  async Task<ApiResult<List<BoxOptionDTO>>> GetBoxOptionsEnableAsync()
        {
            var response = new ApiResult<List<BoxOptionDTO>>();
            List<BoxOptionDTO> boxOptionDTOs = new List<BoxOptionDTO>();

            try
            {
                var boxOptions = await _unitOfWork.BoxOptionRepository.SearchAsync(b => b.IsDeleted == false);

                foreach (var boxOption in boxOptions)
                {
                    var boxOptionDto = _mapper.Map<BoxOptionDTO>(boxOption);
                    boxOptionDTOs.Add(boxOptionDto);
                }

                if (boxOptionDTOs.Count > 0)
                {
                    response.Data = boxOptionDTOs;
                    response.Success = true;
                    response.Message = $"{boxOptionDTOs.Count} BoxOptions are being enable.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No BoxOptions are being enable.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<BoxOptionDTO>>> SearchBoxOptionByNameAsync(string name)
        {
            var response = new ApiResult<List<BoxOptionDTO>>();
            List<BoxOptionDTO> boxOptionDTOs = new List<BoxOptionDTO>();

            try
            {
                var boxOptions = await _unitOfWork.BoxOptionRepository.SearchAsync(b => b.Description.Contains(name));

                foreach (var boxOption in boxOptions)
                {
                    var boxOptionDto = _mapper.Map<BoxOptionDTO>(boxOption);
                    boxOptionDTOs.Add(boxOptionDto);
                }

                if (boxOptionDTOs.Count > 0)
                {
                    response.Data = boxOptionDTOs;
                    response.Success = true;
                    response.Message = $"{boxOptionDTOs.Count} BoxOptions found with the name '{name}'.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No BoxOptions found with the name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<BoxOptionDTO>> UpdateBoxOptionAsync(int id, BoxOptionUpdateDTO updateDto)
        {
            var response = new ApiResult<BoxOptionDTO>();

            try
            {
                var boxOption = await _unitOfWork.BoxOptionRepository.GetByIdAsync(id);

                if (boxOption != null)
                {
                    _mapper.Map(updateDto, boxOption);
                    _unitOfWork.BoxOptionRepository.Update(boxOption);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<BoxOptionDTO>(boxOption);
                        response.Success = true;
                        response.Message = "BoxOption updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update BoxOption.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "BoxOption not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

    }
}

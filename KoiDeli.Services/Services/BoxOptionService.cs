using AutoMapper;
using KoiDeli.Domain.DTOs.BoxOptionDTOs;
using KoiDeli.Domain.DTOs.OrderTimelineDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Domain.Enums;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<ApiResult<List<BoxOptionDTO>>> CreateBoxOptionAsync(BoxOptionCreateRequest boxOptionRequest)
        {
            var response = new ApiResult<List<BoxOptionDTO>>();
            var createdBoxOptions = new List<BoxOption>();

            try
            {
                foreach (var boxData in boxOptionRequest.Boxes)
                {
                    var box = await _unitOfWork.BoxRepository.GetByIdAsync(boxData.BoxId);
                    if (box == null)
                    {
                        response.Success = false;
                        response.Message = $"Box with ID {boxData.BoxId} not found.";
                        return response;
                    }

                    foreach (var fishData in boxData.Fishes)
                    {
                        var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(fishData.FishId);
                        if (koiFish == null)
                        {
                            response.Success = false;
                            response.Message = $"Fish with ID {fishData.FishId} not found.";
                            return response;
                        }

                        var boxOptionDto = new BoxOptionCreateDTO
                        {
                            BoxId = boxData.BoxId,
                            FishId = fishData.FishId,
                            Quantity = fishData.Quantity,
                            Description = $"Added {fishData.Quantity} fish(es) to Box {boxData.BoxId}.",
                            IsChecked = StatusEnum.Pending
                        };

                        var entity = _mapper.Map<BoxOption>(boxOptionDto);

                        entity.IsChecked = boxOptionDto.IsChecked.HasValue
                            ? boxOptionDto.IsChecked.Value.ToString()
                            : StatusEnum.Pending.ToString();

                        await _unitOfWork.BoxOptionRepository.AddAsync(entity);

                        // Add the created BoxOption entity to the list
                        createdBoxOptions.Add(entity);
                    }
                }

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Success = true;
                    response.Message = "BoxOption created successfully.";

                    // Map the created BoxOption entities to BoxOptionDTO and add them to the response data
                    response.Data = _mapper.Map<List<BoxOptionDTO>>(createdBoxOptions);
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

        public async Task<ApiResult<List<BoxOpDTO>>> GetBoxOptionsAsync()
        {
            var response = new ApiResult<List<BoxOpDTO>>();
            List<BoxOpDTO> boxDTOs = new List<BoxOpDTO>();

            try
            {
                // Sử dụng Include để nạp đầy đủ thông tin về Box và KoiFish
                var boxOptions = await _unitOfWork.BoxOptionRepository
                    .GetAll()  // Trả về IQueryable
                    .Include(bo => bo.Box)
                    .Include(bo => bo.Fish)
                    .ToListAsync();  // Thực thi truy vấn và lấy danh sách

                // Nhóm các BoxOptions theo BoxId (mỗi Box có thể chứa nhiều BoxOption)
                var groupedBoxOptions = boxOptions
                    .GroupBy(bo => bo.BoxId)
                    .ToList();

                foreach (var group in groupedBoxOptions)
                {
                    var firstBoxOption = group.First();  // Lấy thông tin Box từ item đầu tiên của nhóm
                    var boxDto = new BoxOpDTO
                    {
                        BoxId = firstBoxOption.BoxId,
                        BoxName = firstBoxOption.Box.Name,
                        MaxVolume = firstBoxOption.Box.MaxVolume,
                        RemainingVolume = firstBoxOption.Box.RemainingVolume,
                        Price = firstBoxOption.Box.Price,
                        UsageCount = group.Count(),
                        TotalFish = (int)group.Sum(g => g.Quantity),
                        TotalVolume = group.Sum(g => g.Fish.Volume * g.Quantity)
                    };

                    // Duyệt qua từng BoxOption để lấy thông tin cá trong hộp
                    foreach (var boxOption in group)
                    {
                        var fishDto = new FishDTO
                        {
                            FishId = boxOption.FishId,
                            FishSize = boxOption.Fish.Size,
                            FishVolume = boxOption.Fish.Volume,
                            FishDescription = boxOption.Fish.Description,
                            Quantity = boxOption.Quantity,
                            BoxOptionId = boxOption.Id  // Thêm Id của BoxOption vào đây
                        };

                        boxDto.Fishes.Add(fishDto);
                    }

                    // Thêm BoxDTO vào danh sách trả về
                    boxDTOs.Add(boxDto);
                }

                if (boxDTOs.Count > 0)
                {
                    response.Data = boxDTOs;
                    response.Success = true;
                    response.Message = $"Found {boxDTOs.Count} boxes with fish details.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No boxes found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }





        public async Task<ApiResult<List<BoxOptionDTO>>> GetBoxOptionsEnableAsync()
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

using AutoMapper;
using KoiDeli.Domain.DTOs.KoiFishDTOs;
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
    public class KoiFishService : IKoiFishService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public KoiFishService(
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

        public async Task<ApiResult<KoiFishDTO>> CreateKoiFishAsync(KoiFishCreateDTO koiFishDto)
        {
            var response = new ApiResult<KoiFishDTO>();

            try
            {
                var entity = _mapper.Map<KoiFish>(koiFishDto);

                await _unitOfWork.KoiFishRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<KoiFishDTO>(entity);
                    response.Success = true;
                    response.Message = "KoiFish created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create KoiFish.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<KoiFishDTO>> DeleteKoiFishAsync(int id)
        {
            var response = new ApiResult<KoiFishDTO>();
            var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);

            if (koiFish != null)
            {
                await _unitOfWork.KoiFishRepository.SoftRemove(koiFish);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<KoiFishDTO>(koiFish);
                    response.Success = true;
                    response.Message = "KoiFish deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete KoiFish.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "KoiFish not found.";
            }

            return response;
        }

        public async Task<ApiResult<List<KoiFishDTO>>> GetKoiFishAsync()
        {
            var response = new ApiResult<List<KoiFishDTO>>();
            List<KoiFishDTO> koiFishDTOs = new List<KoiFishDTO>();

            try
            {
                var koiFishList = await _unitOfWork.KoiFishRepository.GetAllAsync();

                foreach (var koiFish in koiFishList)
                {
                    var koiFishDto = _mapper.Map<KoiFishDTO>(koiFish);
                    koiFishDTOs.Add(koiFishDto);
                }

                if (koiFishDTOs.Count > 0)
                {
                    response.Data = koiFishDTOs;
                    response.Success = true;
                    response.Message = $"Found {koiFishDTOs.Count} KoiFish.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No KoiFish found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<KoiFishDTO>> GetKoiFishByIdAsync(int id)
        {
            var response = new ApiResult<KoiFishDTO>();

            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);

                if (koiFish != null)
                {
                    response.Data = _mapper.Map<KoiFishDTO>(koiFish);
                    response.Success = true;
                    response.Message = "KoiFish retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "KoiFish not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<KoiFishDTO>>> GetKoiFishEnableAsync()
        {
            var response = new ApiResult<List<KoiFishDTO>>();
            List<KoiFishDTO> koiFishDTOs = new List<KoiFishDTO>();

            try
            {
                var koiFishList = await _unitOfWork.KoiFishRepository.SearchAsync(k => k.IsDeleted == false);

                foreach (var koiFish in koiFishList)
                {
                    var koiFishDto = _mapper.Map<KoiFishDTO>(koiFish);
                    koiFishDTOs.Add(koiFishDto);
                }

                if (koiFishDTOs.Count > 0)
                {
                    response.Data = koiFishDTOs;
                    response.Success = true;
                    response.Message = $"{koiFishDTOs.Count} KoiFish are being enable.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No KoiFish are being enable.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<KoiFishDTO>>> SearchKoiFishByNameAsync(string name)
        {
            var response = new ApiResult<List<KoiFishDTO>>();
            List<KoiFishDTO> koiFishDTOs = new List<KoiFishDTO>();

            try
            {
                var koiFishList = await _unitOfWork.KoiFishRepository.SearchAsync(k => k.Description.Contains(name));

                foreach (var koiFish in koiFishList)
                {
                    var koiFishDto = _mapper.Map<KoiFishDTO>(koiFish);
                    koiFishDTOs.Add(koiFishDto);
                }

                if (koiFishDTOs.Count > 0)
                {
                    response.Data = koiFishDTOs;
                    response.Success = true;
                    response.Message = $"{koiFishDTOs.Count} KoiFish found with the name '{name}'.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No KoiFish found with the name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<KoiFishDTO>> UpdateKoiFishAsync(int id, KoiFishUpdateDTO updateDto)
        {
            var response = new ApiResult<KoiFishDTO>();

            try
            {
                var koiFish = await _unitOfWork.KoiFishRepository.GetByIdAsync(id);

                if (koiFish != null)
                {
                    _mapper.Map(updateDto, koiFish);
                    await _unitOfWork.KoiFishRepository.Update(koiFish);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<KoiFishDTO>(koiFish);
                        response.Success = true;
                        response.Message = "KoiFish updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update KoiFish.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "KoiFish not found.";
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

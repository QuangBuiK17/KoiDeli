using AutoMapper;
using KoiDeli.Domain.DTOs.BoxDTOs;
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
    public class BoxService : IBoxService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public BoxService(
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

        public async Task<ApiResult<BoxDTO>> CreateBoxAsync(BoxCreateDTO createdto)
        {
            var reponse = new ApiResult<BoxDTO>();

            try
            {
                var entity = _mapper.Map<Box>(createdto);

                await _unitOfWork.BoxRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    reponse.Data = _mapper.Map<BoxDTO>(entity);
                    reponse.Success = true;
                    reponse.Message = "Create new Box successfully";
                    return reponse;
                }
                else
                {
                    reponse.Success = false;
                    reponse.Message = "Create new Box fail";
                    return reponse;
                }
            }
            catch (Exception ex)
            {
                reponse.Success = false;
                reponse.ErrorMessages = new List<string> { ex.Message };
                return reponse;
            }

            return reponse;
        }

        public async Task<ApiResult<BoxDTO>> DeleteBoxAsync(int id)
        {
            var _response = new ApiResult<BoxDTO>();
            var court = await _unitOfWork.BoxRepository.GetByIdAsync(id);

            if (court != null)
            {
                await _unitOfWork.BoxRepository.SoftRemove(court);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    _response.Data = _mapper.Map<BoxDTO>(court);
                    _response.Success = true;
                    _response.Message = "Deleted Box Successfully!";
                }
                else
                {
                    _response.Success = false;
                    _response.Message = "Deleted Box Fail!";
                }
            }
            else
            {
                _response.Success = false;
                _response.Message = "Box not found";
            }

            return _response;
        }

        public async Task<ApiResult<BoxDTO>> GetBoxByIdAsync(int id)
        {
            var response = new ApiResult<BoxDTO>();
            try
            {
                var box = await _unitOfWork.BoxRepository.GetByIdAsync(id);
                if (box == null)
                {
                    response.Success = false;
                    response.Message = "Don't have any Box";
                }
                else
                {
                    response.Data = _mapper.Map<BoxDTO>(box);
                    response.Success = true;
                    response.Message = "Box retrieved successfully";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }


        public async Task<ApiResult<List<BoxDTO>>> GetBoxesAsync()
        {
            var response = new ApiResult<List<BoxDTO>>();
            List<BoxDTO> CourtDTOs = new List<BoxDTO>();
            try
            {
                var courts = await _unitOfWork.BoxRepository.GetAllAsync();

                foreach (var court in courts)
                {
                    var courtDto = _mapper.Map<BoxDTO>(court);
                    courtDto.Name = court.Name;
                    CourtDTOs.Add(courtDto);
                }
                if (CourtDTOs.Count > 0)
                {
                    response.Data = CourtDTOs;
                    response.Success = true;
                    response.Message = $"Have {CourtDTOs.Count} Boxes.";
                    response.Error = "No error";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Boxes found.";
                    response.Error = "No Boxes available";
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

        public Task<ApiResult<List<BoxDTO>>> SearchBoxByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<BoxDTO>> UpdateBoxAsync(int id, BoxUpdateDTO updatedto)
        {
            var reponse = new ApiResult<BoxDTO>();

            try
            {
                var enityById = await _unitOfWork.BoxRepository.GetByIdAsync(id);

                if (enityById != null)
                {
                    var newb = _mapper.Map(updatedto, enityById);
                    var bAfter = _mapper.Map<Box>(newb);
                    await _unitOfWork.BoxRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        reponse.Success = true;
                        reponse.Data = _mapper.Map<BoxDTO>(bAfter);
                        reponse.Message = $"Successfull for update Box.";
                        return reponse;
                    }
                    else
                    {
                        reponse.Success = false;
                        reponse.Error = "Save update failed";
                        return reponse;
                    }

                }
                else
                {
                    reponse.Success = false;
                    reponse.Message = $"Have no Box.";
                    reponse.Error = "Not have a Box";
                    return reponse;
                }

            }
            catch (Exception ex)
            {
                reponse.Success = false;
                reponse.ErrorMessages = new List<string> { ex.Message };
                return reponse;
            }

            return reponse;
        }
    }
}

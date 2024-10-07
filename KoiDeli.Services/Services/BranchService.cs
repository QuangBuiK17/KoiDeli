using AutoMapper;
using KoiDeli.Domain.DTOs.BranchDTOs;
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
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public BranchService(
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

        public async Task<ApiResult<BranchDTO>> CreateBranchAsync(BranchCreateDTO branchDto)
        {
            var response = new ApiResult<BranchDTO>();

            try
            {
                var entity = _mapper.Map<Branch>(branchDto);

                await _unitOfWork.BranchRepository.AddAsync(entity);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<BranchDTO>(entity);
                    response.Success = true;
                    response.Message = "Branch created successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to create Branch.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<BranchDTO>> DeleteBranchAsync(int id)
        {
            var response = new ApiResult<BranchDTO>();
            var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);

            if (branch != null)
            {
                _unitOfWork.BranchRepository.SoftRemove(branch);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<BranchDTO>(branch);
                    response.Success = true;
                    response.Message = "Branch deleted successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to delete Branch.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Branch not found.";
            }

            return response;
        }

        public async Task<ApiResult<BranchDTO>> GetBranchByIdAsync(int id)
        {
            var response = new ApiResult<BranchDTO>();

            try
            {
                var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);

                if (branch != null)
                {
                    response.Data = _mapper.Map<BranchDTO>(branch);
                    response.Success = true;
                    response.Message = "Branch retrieved successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Branch not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<BranchDTO>>> GetBranchesAsync()
        {
            var response = new ApiResult<List<BranchDTO>>();
            List<BranchDTO> branchDTOs = new List<BranchDTO>();

            try
            {
                var branches = await _unitOfWork.BranchRepository.GetAllAsync();

                foreach (var branch in branches)
                {
                    var branchDto = _mapper.Map<BranchDTO>(branch);
                    branchDTOs.Add(branchDto);
                }

                if (branchDTOs.Count > 0)
                {
                    response.Data = branchDTOs;
                    response.Success = true;
                    response.Message = $"Found {branchDTOs.Count} Branches.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Branches found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<BranchDTO>>> SearchBranchByNameAsync(string name)
        {
            var response = new ApiResult<List<BranchDTO>>();
            List<BranchDTO> branchDTOs = new List<BranchDTO>();

            try
            {
                var branches = await _unitOfWork.BranchRepository.SearchAsync(b => b.Name.Contains(name));

                foreach (var branch in branches)
                {
                    var branchDto = _mapper.Map<BranchDTO>(branch);
                    branchDTOs.Add(branchDto);
                }

                if (branchDTOs.Count > 0)
                {
                    response.Data = branchDTOs;
                    response.Success = true;
                    response.Message = $"{branchDTOs.Count} Branches found with the name '{name}'.";
                }
                else
                {
                    response.Success = false;
                    response.Message = $"No Branches found with the name '{name}'.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<BranchDTO>> UpdateBranchAsync(int id, BranchUpdateDTO updateDto)
        {
            var response = new ApiResult<BranchDTO>();

            try
            {
                var branch = await _unitOfWork.BranchRepository.GetByIdAsync(id);

                if (branch != null)
                {
                    _mapper.Map(updateDto, branch);
                    _unitOfWork.BranchRepository.Update(branch);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Data = _mapper.Map<BranchDTO>(branch);
                        response.Success = true;
                        response.Message = "Branch updated successfully.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Failed to update Branch.";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Branch not found.";
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

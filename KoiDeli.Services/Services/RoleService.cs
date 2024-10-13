using AutoMapper;
using Azure;
using KoiDeli.Domain.DTOs.AccountDTOs;
using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.PartnerShipmentDTOs;
using KoiDeli.Domain.DTOs.RoleDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly AppConfiguration _configuration;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork,
                           ICurrentTime currentTime,
                           AppConfiguration configuration, 
                           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ApiResult<RoleDTO>> CreateRoleAsync(RoleCreateDTO role)
        {
            var reponse = new ApiResult<RoleDTO>();

            try
            {
                var entity = _mapper.Map<Role>(role);
                entity.Name = role.Name;
                await _unitOfWork.RoleRepository.AddAsync(entity);



                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    reponse.Data = _mapper.Map<RoleDTO>(entity);
                    reponse.Success = true;
                    reponse.Message = "Create new Role successfully";
                    return reponse;
                }
                else
                {
                    reponse.Success = false;
                    reponse.Message = "Create new Role fail";
                    return reponse;
                }
            }
            catch (Exception ex)
            {
                reponse.Success = false;
                reponse.ErrorMessages = new List<string> { ex.Message };
                return reponse;
            }
        }

        public async Task<ApiResult<RoleDTO>> DeleteRoleAsync(int id)
        {
            var _response = new ApiResult<RoleDTO>();
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);

            if (role != null)
            {
                _unitOfWork.RoleRepository.SoftRemove(role);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    _response.Data = _mapper.Map<RoleDTO>(role);
                    _response.Success = true;
                    _response.Message = "Deleted Role Successfully!";
                }
                else
                {
                    _response.Success = false;
                    _response.Message = "Deleted Role Fail!";
                }
            }
            else
            {
                _response.Success = false;
                _response.Message = "Role not found";
            }

            return _response;
        }



        public async Task<ApiResult<List<RoleDTO>>> GetRolesAsync()
        {
            var response = new ApiResult<List<RoleDTO>>();
            List<RoleDTO> RolesDTOs = new List<RoleDTO>();
            try
            {
                var roles = await _unitOfWork.RoleRepository.GetAllAsync();

                foreach (var role in roles)
                {
                    var roleDto = _mapper.Map<RoleDTO>(role);
                    roleDto.Name = role.Name;
                    RolesDTOs.Add(roleDto);
                }
                if (RolesDTOs.Count > 0)
                {
                    response.Data = RolesDTOs;
                    response.Success = true;
                    response.Message = $"Have {RolesDTOs.Count} roles.";
                    response.Error = "No error";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No roles found.";
                    response.Error = "No roles available";
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

        public async Task<ApiResult<List<RoleDTO>>> GetRolesEnabledAsync()
        {
            var response = new ApiResult<List<RoleDTO>>();
            List<RoleDTO> RolesDTOs = new List<RoleDTO>();
            try
            {
                var roles = await _unitOfWork.RoleRepository.GetRolesEnabledAsync();

                foreach (var role in roles)
                {
                    var roleDto = _mapper.Map<RoleDTO>(role);
                    roleDto.Name = role.Name;
                    RolesDTOs.Add(roleDto);
                }
                if (RolesDTOs.Count > 0)
                {
                    response.Data = RolesDTOs;
                    response.Success = true;
                    response.Message = $"Have {RolesDTOs.Count} roles enabled.";
                    response.Error = "No error";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No roles found.";
                    response.Error = "No roles available";
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

        public async Task<ApiResult<RoleDTO>> UpdateRoleAsync(int id, RoleUpdateDTO updateDto)
        {
            var reponse = new ApiResult<RoleDTO>();

            try
            {
                var enityById = await _unitOfWork.RoleRepository.GetByIdAsync(id);

                if (enityById != null)
                {
                    var newb = _mapper.Map(updateDto, enityById);
                    var bAfter = _mapper.Map<Role>(newb);
                    _unitOfWork.RoleRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        reponse.Success = true;
                        reponse.Data = _mapper.Map<RoleDTO>(bAfter);
                        reponse.Message = $"Successfull for update Role.";
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
                    reponse.Message = $"Have no Role.";
                    reponse.Error = "Not have a Role";
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

        public async Task<ApiResult<RoleDTO>> GetRoleByIdAsync(int id)
        {
            var response = new ApiResult<RoleDTO>();
            try
            {
                var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
                if (role == null)
                {
                    response.Success = false;
                    response.Message = "Role ID doesn't exit!";
                    return response;
                }
                else
                {
                    response.Data = _mapper.Map<RoleDTO>(role);
                    response.Success = true;
                    response.Message = "Role ID is valid";
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



        public async Task<ApiResult<List<RoleDTO>>> SearchRoleByNameAsync(string name)
        {
            var response = new ApiResult<List<RoleDTO>>();
            List<RoleDTO> RoleDTOs = new List<RoleDTO>();
            try
            {
                var roles = await _unitOfWork.RoleRepository.GetRoleByNameAsync(name);
                foreach (var role in roles)
                {
                    var roleDto = _mapper.Map<RoleDTO>(role);
                    RoleDTOs.Add(roleDto);
                }
                if (RoleDTOs.Count > 0)
                {
                    response.Data = RoleDTOs;
                    response.Success = true;
                    response.Message = $"Have {RoleDTOs.Count} role enabled.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "Name not found or have been deleted";
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
    }
}

using AutoMapper;
using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.DTOs.UserDTOs;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResult<UserDTO>> CreateAsync(UserCreateDTO creaeDTO)
        {
            var response = new ApiResult<UserDTO>();
            try
            {

                var user = _mapper.Map<User>(creaeDTO);
                await _unitOfWork.UserRepository.AddAsync(user);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<UserDTO>(user);
                    response.Success = true;
                    response.Message = "Create new User successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Create new User fail";
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

        public async  Task<ApiResult<UserDTO>> DeleteAsync(int id)
        {
            var response = new ApiResult<UserDTO>();
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                response.Success = false;
                response.Message = "user not found!";
                return response;
            }
            else
            {
                _unitOfWork.UserRepository.SoftRemove(user);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<UserDTO>(user);
                    response.Success = true;
                    response.Message = "Delete user successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Delete user fail";
                    return response;
                }
            }
        }

        public async Task<ApiResult<List<UserDTO>>> GetAsync()
        {
            var response = new ApiResult<List<UserDTO>>();
            List<UserDTO> UserDTOs = new List<UserDTO>();
            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                foreach (var user in users)
                {
                    var userDto = _mapper.Map<UserDTO>(user);

                    UserDTOs.Add(userDto);
                }
                if (UserDTOs.Count > 0)
                {
                    response.Data = UserDTOs;
                    response.Success = true;
                    response.Message = $"Have {UserDTOs.Count} users.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No user created";
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

        public async Task<ApiResult<UserDTO>> GetByIdAsync(int id)
        {
            var response = new ApiResult<UserDTO>();
            try
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User ID doesn't exit!";
                    return response;
                }
                else
                {
                    response.Data = _mapper.Map<UserDTO>(user);
                    response.Success = true;
                    response.Message = "user ID is valid";
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

        public async Task<ApiResult<UserDTO>> UpdatetAsync(int id, UserUpdateDTO updateDto)
        {
            var response = new ApiResult<UserDTO>();

            try
            {
                var enityById = await _unitOfWork.UserRepository.GetByIdAsync(id);

                if (enityById != null)
                {

                    var newb = _mapper.Map(updateDto, enityById);
                    var bAfter = _mapper.Map<User>(newb);
                    _unitOfWork.UserRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        response.Data = _mapper.Map<UserDTO>(bAfter);
                        response.Message = $"Successfull for update user.";
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
                    response.Message = $"Have no user.";
                    response.Error = "Not have a user";
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

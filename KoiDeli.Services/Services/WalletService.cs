using AutoMapper;
using Azure;
using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.VnPayDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Domain.Enums;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ApiResult<WalletDTO>> CreateWalletAsync(WalletCreateDTO creaeDTO)
        {
            var response = new ApiResult<WalletDTO>();
            try
            {
                if (!await _unitOfWork.WalletRepository.CheckUserIdExisted(creaeDTO.UserId))
                {
                    response.Success = false;
                    response.Message = "UserID is invalid";
                    return response;
                }
                var wallet = _mapper.Map<Wallet>(creaeDTO);
                wallet.Balance = 0;
                await _unitOfWork.WalletRepository.AddAsync(wallet);
                if ( await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<WalletDTO>(wallet);
                    response.Success = true;
                    response.Message = "Create new Wallet successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Create new Wallet fail";
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

        public async Task<ApiResult<WalletDTO>> DeleteWalletAsync(int id)
        {
            var response = new ApiResult<WalletDTO>();
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
            if (wallet == null)
            {
                response.Success = false;
                response.Message = "wallet not found!";
                return response;
            }
            else
            {
                _unitOfWork.WalletRepository.SoftRemove(wallet);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<WalletDTO>(wallet);
                    response.Success = true;
                    response.Message = "Delete Wallet successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Delete wallet fail";
                    return response;
                }
            }
        }

        

        public async Task<ApiResult<List<WalletDTO>>> GetWalletAsync()
        {
            var response = new ApiResult<List<WalletDTO>>();
            List<WalletDTO> WalletsDTOs = new List<WalletDTO>();
            try
            {
                var wallets = await _unitOfWork.WalletRepository.GetAllAsync();
                foreach (var wallet in wallets)
                {
                    var walletDto = _mapper.Map<WalletDTO>(wallet);

                    WalletsDTOs.Add(walletDto);
                }
                if (WalletsDTOs.Count > 0)
                {
                    response.Data = WalletsDTOs;
                    response.Success = true;
                    response.Message = $"Have {WalletsDTOs.Count} wallets.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No wallet created";
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

        public async Task<ApiResult<WalletDTO>> GetWalletByIdAsync(int id)
        {
            var response = new ApiResult<WalletDTO>();
            try
            {
                var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(id);
                if (wallet == null)
                {
                    response.Success = false;
                    response.Message = "Wallet ID doesn't exit!";
                    return response;
                }
                else
                {
                    response.Data = _mapper.Map<WalletDTO>(wallet);
                    response.Success = true;
                    response.Message = "Wallet ID is valid";
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

        public async Task<ApiResult<List<WalletDTO>>> GetWalletEnabledAsync()
        {
            var response = new ApiResult<List<WalletDTO>>();
            List<WalletDTO> WalletsDTOs = new List<WalletDTO>();
            try
            {
                var wallets = await _unitOfWork.WalletRepository.GetWalletssEnabledAsync();
                foreach (var wallet in wallets)
                {
                    var walletDto = _mapper.Map<WalletDTO>(wallet);

                    WalletsDTOs.Add(walletDto);
                }
                if (WalletsDTOs.Count > 0)
                {
                    response.Data = WalletsDTOs;
                    response.Success = true;
                    response.Message = $"Have {WalletsDTOs.Count} wallets enabled.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No wallet created";
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

        public async Task<ApiResult<WalletDTO>> UpdateWalletAsync(int id, WalletUpdateDTO updateDto)
        {
            var response = new ApiResult<WalletDTO>();

            try
            {
                var enityById = await _unitOfWork.WalletRepository.GetByIdAsync(id);

                if (enityById != null)
                {
                    // kiểm tra userid
                    if (!await _unitOfWork.WalletRepository.CheckUserIdExisted(updateDto.UserId))
                    {
                        response.Success = false;
                        response.Message = "UserID is invalid";
                        return response;
                    }
                    //kiểm tra balance
                    if(updateDto.Balance < 0)
                    {
                        response.Success = false;
                        response.Message = "Balance must be positive";
                        return response;
                    }

                    var newb = _mapper.Map(updateDto, enityById);
                    var bAfter = _mapper.Map<Wallet>(newb);
                    _unitOfWork.WalletRepository.Update(bAfter);
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        response.Data = _mapper.Map<WalletDTO>(bAfter);
                        response.Message = $"Successfull for update wallet.";
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
                    response.Message = $"Have no wallet.";
                    response.Error = "Not have a wallet";
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

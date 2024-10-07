using AutoMapper;
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
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResult<TransactionDTO>> CreateAsync(TransactionCreateDTO creaeDTO)
        {
            var response = new ApiResult<TransactionDTO>();
            try
            {
                // Kiểm tra xem WalletId có hợp lệ không

                var wallet = await _unitOfWork.TransactionRepository.CheckWalletExisted(creaeDTO.WalletId);
                if (wallet == null)
                {
                    response.Success = false;
                    response.Message = "WalletID is invalid";
                    return response;
                }
                var transaction = new Transaction
                {
                    TotalAmount = creaeDTO.TotalAmount,
                    PaymentType = creaeDTO.PaymentType,
                    Wallet = wallet
                };
                await _unitOfWork.TransactionRepository.AddAsync(transaction);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<TransactionDTO>(transaction);
                    response.Success = true;
                    response.Message = "Create new transaction successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Create new transaction fail";
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

        /*
public async Task<ApiResult<TransactionDTO>> CreateAsync(TransactionCreateDTO creaeDTO)
{
   var response = new ApiResult<TransactionDTO>();
   try
   {
       if (!await _unitOfWork.TransactionRepository.CheckWalletIdExisted(creaeDTO.WalletId))
       {
           response.Success = false;
           response.Message = "WalletID is invalid";
           return response;
       }
       var transaction = _mapper.Map<Transaction>(creaeDTO);
       await _unitOfWork.TransactionRepository.AddAsync(transaction);
       if (await _unitOfWork.SaveChangeAsync() > 0)
       {
           response.Data = _mapper.Map<TransactionDTO>(transaction);
           response.Success = true;
           response.Message = "Create new transaction successfully";
           return response;
       }
       else
       {
           response.Success = false;
           response.Message = "Create new transaction fail";
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
*/
        public async Task<ApiResult<TransactionDTO>> DeleteAsync(int id)
        {
            var response = new ApiResult<TransactionDTO>();
            var transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                response.Success = false;
                response.Message = "transaction not found!";
                return response;
            }
            else
            {
                _unitOfWork.TransactionRepository.SoftRemove(transaction);
                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    response.Data = _mapper.Map<TransactionDTO>(transaction);
                    response.Success = true;
                    response.Message = "Delete Transaction successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Delete transaction fail";
                    return response;
                }
            }
        }
        /*
        public async Task<ApiResult<List<TransactionDTO>>> GetAsync()
        {
            var response = new ApiResult<List<TransactionDTO>>();
            List<TransactionDTO> TransactionsDTOs = new List<TransactionDTO>();
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetAllAsync();
                foreach (var transaction in transactions)
                {

                    
                     var transactionDto = _mapper.Map<TransactionDTO>(transaction);
                    // Kiểm tra nếu Wallet không null
                    if (transaction.Wallet != null)
                    {
                        transactionDto.WalletId = transaction.Wallet.Id;
                    }
                    TransactionsDTOs.Add(transactionDto);
                    
                    
                }
                if (TransactionsDTOs.Count > 0)
                {
                    response.Data = TransactionsDTOs;
                    response.Success = true;
                    response.Message = $"Have {TransactionsDTOs.Count} transactions.";
                    response.Error = "No error";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No transaction created";
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
        }*/

        public async Task<ApiResult<List<TransactionDTO>>> GetAsync()
        {
            var response = new ApiResult<List<TransactionDTO>>();
            List<TransactionDTO> TransactionsDTOs = new List<TransactionDTO>();
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetTransactionInfoAsync();
                if (transactions.Count > 0)
                {
                    response.Data = transactions; 
                    response.Success = true;
                    response.Message = $"Found {transactions.Count} transactions.";
                    response.Error = "No error";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No transactions found.";
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

        public async Task<ApiResult<TransactionDTO>> GetByIdAsync(int id)
        {
            var response = new ApiResult<TransactionDTO>();
            try
            {
                var transaction = await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(id);

                // Kiểm tra nếu transaction null hoặc đã bị xóa (IsDeleted = true)
                if (transaction == null || transaction.IsDeleted)
                {
                    response.Success = false;
                    response.Message = "Transaction ID doesn't exist or has been deleted!";
                    return response;
                }
                var transactionDto = _mapper.Map<TransactionDTO>(transaction);
                transactionDto.WalletId = transaction.Wallet.Id; // Lấy WalletId từ đối tượng Wallet

                // Map transaction sang DTO và trả về kết quả
                response.Data = transactionDto;
                response.Success = true;
                response.Message = "Transaction ID is valid";
            }
            catch (Exception ex)
            {
                // Bắt lỗi nếu có
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }

        public async Task<ApiResult<List<TransactionDTO>>> GetEnabledAsync()
        {
            var response = new ApiResult<List<TransactionDTO>>();
            List<TransactionDTO> TransactionsDTOs = new List<TransactionDTO>();
            try
            {
                var transactions = await _unitOfWork.TransactionRepository.GetTransactionsEnabledAsync();
                if (transactions.Count > 0)
                {
                    response.Data = transactions;
                    response.Success = true;
                    response.Message = $"Found {transactions.Count} transactions.";
                    response.Error = "No error";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No transactions found.";
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
        /*
        public async Task<ApiResult<TransactionDTO>> UpdatetAsync(int id, TransactionUpdateDTO updateDto)
        {
            var response = new ApiResult<TransactionDTO>();

            try
            {
                // Lấy transaction hiện tại theo ID
                //var entityById = await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(id);
                var entityById = await _unitOfWork.TransactionRepository.GetByIdAsync(id);

                if (entityById != null && !entityById.IsDeleted) 
                {
                    // Kiểm tra WalletId
                    if (!await _unitOfWork.TransactionRepository.CheckWalletIdExisted(updateDto.WalletId))
                    {
                        response.Success = false;
                        response.Message = "WalletId is invalid";
                        return response;
                    }

                    // Kiểm tra TotalAmount
                    if (updateDto.TotalAmount < 0)
                    {
                        response.Success = false;
                        response.Message = "Total amount must be positive";
                        return response;
                    }

                    // Mapping thông tin từ DTO sang entity hiện tại
                    var updatedTransaction = _mapper.Map(updateDto, entityById);
                    var entity = _mapper.Map<Transaction>(updatedTransaction);
                    // Cập nhật transaction
                    _unitOfWork.TransactionRepository.Update(entity);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        response.Data = _mapper.Map<TransactionDTO>(entity); 
                        response.Message = "Successfully updated the transaction.";
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
                    response.Message = "Transaction not found or has been deleted.";
                    response.Error = "Transaction not found";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }*/

        public async Task<ApiResult<TransactionDTO>> UpdatetAsync(int id, TransactionUpdateDTO updateDto)
        {
            var response = new ApiResult<TransactionDTO>();

            try
            {
                var enityById = await _unitOfWork.TransactionRepository.GetTransactionByIdAsync(id);


                if (enityById != null && !enityById.IsDeleted)
                {
                    // Kiểm tra WalletId
                    if (!await _unitOfWork.TransactionRepository.CheckWalletIdExisted(updateDto.WalletId))
                    {
                        response.Success = false;
                        response.Message = "WalletId is invalid";
                        return response;
                    }

                    // Kiểm tra TotalAmount
                    if (updateDto.TotalAmount < 0)
                    {
                        response.Success = false;
                        response.Message = "Total amount must be positive";
                        return response;
                    }

                    // Mapping thông tin từ DTO sang entity hiện tại
                    var updatedTransaction = _mapper.Map(updateDto, enityById);
                    var entity = _mapper.Map<Transaction>(updatedTransaction);
                    // Cập nhật transaction
                    _unitOfWork.TransactionRepository.Update(entity);

                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        response.Success = true;
                        response.Data = _mapper.Map<TransactionDTO>(entity);
                        response.Message = "Successfully updated the transaction.";
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
                    response.Message = "Transaction not found or has been deleted.";
                    response.Error = "Transaction not found";
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

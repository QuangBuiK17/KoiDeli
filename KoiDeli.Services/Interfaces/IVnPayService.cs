﻿using KoiDeli.Domain.DTOs.VnPayDTOs;
using KoiDeli.Services.Services.VnPayConfig;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreateLink(VnpayOrderInfo orderInfo);

        PaymentResponseModel GetFullResponseData(IQueryCollection collection);

        Task<IPNReponse> IPNReceiver(string vnpTmnCode, string vnpSecureHash, string vnpTxnRef, string vnpTransactionStatus, string vnpResponseCode, string vnpTransactionNo, string vnpBankCode, string vnpAmount, string vnpPayDate, string vnpBankTranNo, string vnpCardType, NameValueCollection requestNameValue);
    }
}

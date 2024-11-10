using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Domain.DTOs.VnPayDTOs
{
    public class PaymentResponseModel
    {
        public string OrderDescription { get; set; }
        public string TransactionToken { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionNo { get; set; }
        public bool Success { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }

        public decimal? AmountOfMoney { get; set; }

        public string BanKTranNo { get; set; }

        public DateTime PayDate { get; set; }
    }

    public class VnpayResponseDTO
    {
        public string vnp_TmnCode { get; set; } = string.Empty;
        public string vnp_BankCode { get; set; } = string.Empty;
        public string vnp_BankTranNo { get; set; } = string.Empty;
        public string vnp_CardType { get; set; } = string.Empty;
        public string vnp_OrderInfo { get; set; } = string.Empty;
        public string vnp_TransactionNo { get; set; } = string.Empty;
        public string vnp_TransactionStatus { get; set; } = string.Empty;
        public string? vnp_TxnRef { get; set; }
        public string vnp_SecureHashType { get; set; } = string.Empty;
        public string vnp_SecureHash { get; set; } = string.Empty;
        public string vnp_Amount { get; set; }
        public string? vnp_ResponseCode { get; set; }
        public string vnp_PayDate { get; set; } = string.Empty;
    }
    public class DepositResponseDTO
    {
        public string? PayUrl { get; set; }
    }
}

using KoiDeli.Domain.DTOs.DistanceDTOs;
using KoiDeli.Domain.DTOs.VnPayDTOs;
using KoiDeli.Domain.DTOs.WalletDTOs;
using KoiDeli.Domain.Enums;
using KoiDeli.Repositories.Common;
using KoiDeli.Repositories.Interfaces;
using KoiDeli.Services.Interfaces;
using KoiDeli.Services.Services;
using KoiDeli.Services.Services.VnPayConfig;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KoiDeli.Controllers
{
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;
        private readonly IVnPayService _vnPayService;
        public WalletController(IWalletService walletService, IVnPayService vnPayService)
        {
            _walletService = walletService;
            _vnPayService = vnPayService;
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllWallets()
        {
            var result = await _walletService.GetWalletAsync();
            return Ok(result);
        }

        [HttpGet("enable")]
        public async Task<IActionResult> ViewAllWalletsEnabled()
        {
            var result = await _walletService.GetWalletEnabledAsync();
            return Ok(result);
        }

        [HttpGet("id")]
        public async Task<IActionResult> ViewWalletById(int id)
        {
            var result = await _walletService.GetWalletByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody] WalletCreateDTO createDto)
        {
            if (createDto == null)
            {
                return BadRequest();
            }
            var w = await _walletService.CreateWalletAsync(createDto);
            if (!w.Success)
            {
                return BadRequest();
            }
            return Ok(w);
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateWallet(int id, [FromBody] WalletUpdateDTO updateDto)
        {
            var w = await _walletService.UpdateWalletAsync(id, updateDto);
            if (!w.Success)
            {
                return BadRequest(w);
            }
            return Ok(w);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var w  = await _walletService.DeleteWalletAsync(id);
            if (!w.Success)
            {
                return BadRequest(w);
            }
            return Ok(w);
        }

        [HttpPost("wallets/deposit")]
        public async Task<IActionResult> DepositAsync(long amount, int CommonId)
        {
            try
            {
                // Define order information directly without calling the wallet service
                var orderInfo = new VnpayOrderInfo
                {
                    Amount = amount,
                    Description = "Deposit for Wallet",
                    CommonId = CommonId//new Random().Next(100000, 999999)  Generate a unique transaction reference (you may want to replace this with a more reliable ID generation logic)
                };

                // Generate the VNPay payment link
                var paymentUrl = _vnPayService.CreateLink(orderInfo);

                // Populate response DTO with transaction details and VNPay URL
                var depositResponse = new DepositResponseDTO
                {
              
                    PayUrl = paymentUrl
                };

                return Ok(ServiceResponse<DepositResponseDTO>.Succeed(depositResponse, "Create deposit order successfully"));
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        /// <summary>
        /// [DON'T TOUCH] VnPay IPN Receiver [FromQuery] VnpayResponseModel vnpayResponseModel
        /// </summary>
        [HttpGet("payment/vnpay-ipn-receive")]
        public async Task<IActionResult> PaymentReturn([FromQuery] VnpayResponseDTO vnpayResponseModel)
        {
            try
            {
                var htmlString = string.Empty;
                var requestNameValue = HttpUtility.ParseQueryString(HttpContext.Request.QueryString.ToString());

                IPNReponse iPNReponse = await _vnPayService.IPNReceiver(
                    vnpayResponseModel.vnp_TmnCode,
                    vnpayResponseModel.vnp_SecureHash,
                    vnpayResponseModel.vnp_TxnRef,
                    vnpayResponseModel.vnp_TransactionStatus,
                    vnpayResponseModel.vnp_ResponseCode,
                    vnpayResponseModel.vnp_TransactionNo,
                    vnpayResponseModel.vnp_BankCode,
                    vnpayResponseModel.vnp_Amount,
                    vnpayResponseModel.vnp_PayDate,
                    vnpayResponseModel.vnp_BankTranNo,
                    vnpayResponseModel.vnp_CardType, requestNameValue);


                //Get root path and read index.html
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "index.html");

                using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        htmlString = sr.ReadToEnd();
                    }
                }
                string orderInfo = vnpayResponseModel.vnp_OrderInfo ?? "Không có thông tin";
                //format html
                var isSuccess = iPNReponse.status.ToString() == TransactionStatusEnums.COMPLETED.ToString();
                var textColor = isSuccess ? "text-green-500 dark:text-green-300" : "text-red-500 dark:text-red-300";
                var statusHTML = $"<p class=\"mt-1 text-md {textColor}\">{iPNReponse.status.ToString()}</p>";

                // Send notification
                //var notification = new Notification
                //{
                //    Title = "Deposit " + int.Parse(iPNReponse.price) / 100,
                //    Body = iPNReponse.message,
                //    UserId = _claimsService.GetCurrentUserId == Guid.Empty ? Guid.Empty : _claimsService.GetCurrentUserId,
                //    Url = "/profile/wallet",
                //    Sender = "System"
                //};
                //await _notificationService.PushNotification(notification).ConfigureAwait(true);

                //format image
                var imageHTML = string.Empty;
                if (isSuccess)
                {
                    imageHTML = $"<!--green: from-[#00b894] to-[#55efc4] -->\r\n                <!-- red: from-[#FF4B4B] to-[#FF8B8B] -->\r\n                <div class=\"absolute inset-0 bg-gradient-to-br from-[#00b894] to-[#55efc4] rounded-lg shadow-lg\">\r\n                    <div class=\"flex flex-col items-center justify-center h-full text-white\">\r\n                        <div class=\"text-6xl font-bold star\">✨</div>\r\n                        <!-- <div className=\"text-6xl font-bold hidden\">❌</div> -->\r\n                        <div class=\"wrapper\">\r\n                            <h1 class=\"mt-4 text-2xl font-bold\">Payment Successful</h1>\r\n                        </div>\r\n                    </div>\r\n                </div>";
                }
                else
                {
                    imageHTML = "<!--green: from-[#00b894] to-[#55efc4] -->\r\n                <!-- red: from-[#FF4B4B] to-[#FF8B8B] -->\r\n                <div class=\"absolute inset-0 bg-gradient-to-br from-[#FF4B4B] to-[#FF8B8B] rounded-lg shadow-lg\">\r\n                    <div class=\"flex flex-col items-center justify-center h-full text-white\">\r\n                        \r\n                        <div className=\"text-6xl font-bold hidden\">❌</div>\r\n                        <div class=\"wrapper\">\r\n                            <h1 class=\"mt-4 text-2xl font-bold\">Payment Failed</h1>\r\n                        </div>\r\n                    </div>\r\n                </div>>";
                }

                string htmlFormat = string.Format(htmlString, imageHTML, iPNReponse.transactionId.ToString(), $"{int.Parse(iPNReponse.price) / 100}", statusHTML, iPNReponse.message);

                return Content(htmlFormat, "text/html");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("400"))
                    return BadRequest(ServiceResponse<object>.Fail(ex));
                if (ex.Message.Contains("404"))
                    return NotFound(ServiceResponse<object>.Fail(ex));
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        private IActionResult HandleError(Exception ex)
        {
            if (ex.Message.Contains("400"))
                return BadRequest(ServiceResponse<object>.Fail(ex));
            if (ex.Message.Contains("404"))
                return NotFound(ServiceResponse<object>.Fail(ex));
            if (ex.Message.Contains("401"))
                return Unauthorized(ServiceResponse<object>.Fail(ex));
            if (ex.Message.Contains("403"))
                return StatusCode(StatusCodes.Status403Forbidden, ServiceResponse<object>.Fail(ex));

            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }
}

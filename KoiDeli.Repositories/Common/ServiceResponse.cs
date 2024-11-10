using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Common
{
    public record ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public static ServiceResponse<T> Succeed(T? data, string message)
        {
            return new ServiceResponse<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ServiceResponse<T> Error(T? data, string Message)
        {
            return new ServiceResponse<T> { IsSuccess = false, Data = data, Message = Message };
        }

        public static ServiceResponse<object> Fail(Exception ex)
        {
            return new ServiceResponse<object>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }
    }
    
}


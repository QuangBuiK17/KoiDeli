using KoiDeli.Domain.DTOs.FeedbackDTOs;
using KoiDeli.Domain.DTOs.OrderDTOs;
using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<bool> CheckOrderIdExisted(int id);
        Task<Order> ChecOrderExisted(int id);
        Task<int> GetOrderIdByFeedbackIdAsync(int feedbackID);
        Task<List<FeedbackDTO>> GetFeedbackInfoAsync();
        Task<Feedback> GetFeedbackByIdAsync(int id);
    }
}

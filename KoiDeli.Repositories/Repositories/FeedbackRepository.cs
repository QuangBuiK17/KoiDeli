using KoiDeli.Domain.DTOs.FeedbackDTOs;
using KoiDeli.Domain.DTOs.OrderDTOs;
using KoiDeli.Domain.DTOs.TransactionDTOs;
using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        KoiDeliDbContext _context;
        public FeedbackRepository(KoiDeliDbContext context, 
                                  ICurrentTime timeService,
                                  IClaimsService claimsService) 
            : base(context, timeService, claimsService)
        {
            _context = context;
        }

        public async Task<bool> CheckOrderIdExisted(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        public async Task<Order> ChecOrderExisted(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            var feedback = await _context.Feedbacks
                .Where(f => f.Id == id && f.IsDeleted == false)
                .Select(f => new Feedback
                {
                    Id = f.Id,
                    IsDeleted = f.IsDeleted,
                    Desciption = f.Desciption,
                    Order = new Order
                    {
                        Id = f.Order.Id,
                    }
                })
                .FirstOrDefaultAsync(); // Trả về transaction đầu tiên (hoặc null nếu không có)

            return feedback;
        }

        public async Task<int> GetOrderIdByFeedbackIdAsync(int feedbackID)
        {
            var feedback = await _context.Feedbacks
                .Where(f => f.Id == feedbackID && !f.IsDeleted) 
                .FirstOrDefaultAsync();
            if (feedback == null || feedback.Order.Id == null)
            {
                return 0;
            }
            return feedback.Order.Id;
        }

        public async Task<List<FeedbackDTO>> GetFeedbackInfoAsync()
        {
            var feedbackInfoList = await _context.Feedbacks
                .Select(feedback => new FeedbackDTO
                {
                    Id = feedback.Id,
                    IsDeleted = feedback.IsDeleted,
                    Desciption = feedback.Desciption,
                    OrderId = feedback.Order.Id,
                })
                .ToListAsync();

            return feedbackInfoList;
        }

        public async Task<List<FeedbackDTO>> GetFeedbacksEnabledAsync()
        {
            var feedbackInfoList = await _context.Feedbacks
                .Where(feedback => feedback.IsDeleted == false) // Thêm điều kiện isDeleted = false
                .Select(feedback => new FeedbackDTO
                {
                    Id = feedback.Id,
                    IsDeleted = feedback.IsDeleted,
                    Desciption = feedback.Desciption,
                    OrderId = feedback.Order.Id,
                })
                .ToListAsync();

            return feedbackInfoList;
        }
    }
}

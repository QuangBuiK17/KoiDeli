using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        // add Interface vao day
        IAccountRepository AccountRepository { get; }
        IKoiFishRepository KoiFishRepository { get; }
        IBoxRepository BoxRepository { get; }
        IBoxOptionRepository BoxOptionRepository { get; }

        IVehicleRepository VehicleRepository { get; }
        IBranchRepository BranchRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }

        ITimelineDeliveryRepository TimelineDeliveryRepository { get; }
        IOrderTimelineRepository OrderTimelineRepository { get; }
        IRoleRepository RoleRepository { get; }
        IDistanceRepository DistanceRepository { get; }
        IPartnerShipmentRepository PartnerShipmentRepository { get; }
        IWalletRepository WalletRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IUserRepository UserRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }



        Task<int> SaveChangeAsync();
    }
}

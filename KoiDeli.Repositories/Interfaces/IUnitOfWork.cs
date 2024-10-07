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
        IPartnerShipmentRepository PartnerShipmentRepository { get; }

        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }

        
       


        Task<int> SaveChangeAsync();
    }
}

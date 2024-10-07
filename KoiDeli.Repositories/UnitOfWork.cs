

using KoiDeli.Repositories.Interfaces;

namespace KoiDeli.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        // khai bao IRepository vao day
        private readonly KoiDeliDbContext _koiDeliDbContext;
        private readonly IAccountRepository _accountRepository;
        private readonly IBoxRepository _boxRepository;
        private readonly IBoxOptionRepository _boxOptionRepository;
        private readonly IKoiFishRepository _koiFishRepository;
        private readonly IPartnerShipmentRepository _partnerShipmentRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderTimelineRepository _orderTimelineRepository;
        private readonly ITimelineDeliveryRepository _timelineDeliveryRepository;

        // add vao UnitOfWork
        public UnitOfWork(KoiDeliDbContext koiDeliDbContext,
            IAccountRepository accountRepository,
            IBoxRepository boxRepository,
            IBoxOptionRepository boxOptionRepository,
            IKoiFishRepository koiFishRepository,
            IPartnerShipmentRepository partnerShipmentRepository,
            IVehicleRepository vehicleRepository,
            IBranchRepository branchRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IOrderTimelineRepository orderTimelineRepository,
            ITimelineDeliveryRepository timelineDeliveryRepository)
        {
            _koiDeliDbContext = koiDeliDbContext;
            _accountRepository = accountRepository;
            _boxRepository = boxRepository;
            _boxOptionRepository = boxOptionRepository;
            _koiFishRepository = koiFishRepository;
            _partnerShipmentRepository = partnerShipmentRepository;
            _vehicleRepository = vehicleRepository;
            _branchRepository = branchRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _timelineDeliveryRepository = timelineDeliveryRepository;
            _orderTimelineRepository = orderTimelineRepository;
        }


        public IAccountRepository AccountRepository => _accountRepository;
        public IBoxRepository BoxRepository => _boxRepository;
        public IBoxOptionRepository BoxOptionRepository => _boxOptionRepository;    
        public IKoiFishRepository KoiFishRepository => _koiFishRepository;
        public IBranchRepository BranchRepository => _branchRepository;
        public IVehicleRepository VehicleRepository => _vehicleRepository;
        public IPartnerShipmentRepository PartnerShipmentRepository => _partnerShipmentRepository;

        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;
        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderTimelineRepository OrderTimelineRepository => _orderTimelineRepository;
        public ITimelineDeliveryRepository TimelineDeliveryRepository => _timelineDeliveryRepository;



        //save changes
        public Task<int> SaveChangeAsync()
        {
            try
            {
                return _koiDeliDbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

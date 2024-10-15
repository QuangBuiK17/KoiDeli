

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
        private readonly IBranchRepository _branchRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderTimelineRepository _orderTimelineRepository;
        private readonly ITimelineDeliveryRepository _timelineDeliveryRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IDistanceRepository _distanceRepository;
      
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFeedbackRepository _feedbackRepository;

        // add vao UnitOfWork
        public UnitOfWork(KoiDeliDbContext koiDeliDbContext,
            IAccountRepository accountRepository,
            IBoxRepository boxRepository,
            IBoxOptionRepository boxOptionRepository,
            IKoiFishRepository koiFishRepository,
            IVehicleRepository vehicleRepository,
            IBranchRepository branchRepository,
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            IOrderTimelineRepository orderTimelineRepository,
            ITimelineDeliveryRepository timelineDeliveryRepository,
            IRoleRepository roleRepository,
            IDistanceRepository distanceRepository,
            
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository,
            IUserRepository userRepository,
            IFeedbackRepository feedbackRepository)
        {
            _koiDeliDbContext = koiDeliDbContext;
            _accountRepository = accountRepository;
            _boxRepository = boxRepository;
            _boxOptionRepository = boxOptionRepository;
            _koiFishRepository = koiFishRepository;
            
            _vehicleRepository = vehicleRepository;
            _branchRepository = branchRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _timelineDeliveryRepository = timelineDeliveryRepository;
            _orderTimelineRepository = orderTimelineRepository;
            _roleRepository = roleRepository;
            _distanceRepository = distanceRepository;
            
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _feedbackRepository = feedbackRepository;
        }


        public IAccountRepository AccountRepository => _accountRepository;
        public IBoxRepository BoxRepository => _boxRepository;
        public IBoxOptionRepository BoxOptionRepository => _boxOptionRepository;    
        public IKoiFishRepository KoiFishRepository => _koiFishRepository;
        public IBranchRepository BranchRepository => _branchRepository;
        public IVehicleRepository VehicleRepository => _vehicleRepository;
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;
        public IOrderRepository OrderRepository => _orderRepository;

        public IOrderTimelineRepository OrderTimelineRepository => _orderTimelineRepository;
        public ITimelineDeliveryRepository TimelineDeliveryRepository => _timelineDeliveryRepository;
        public IRoleRepository RoleRepository => _roleRepository;
        public IDistanceRepository DistanceRepository => _distanceRepository;
       
        public IWalletRepository WalletRepository => _walletRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;
        public IUserRepository UserRepository => _userRepository;
        public IFeedbackRepository FeedbackRepository => _feedbackRepository;



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

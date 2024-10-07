

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

        // add vao UnitOfWork
        public UnitOfWork(KoiDeliDbContext koiDeliDbContext,
            IAccountRepository accountRepository,
            IBoxRepository boxRepository,
            IBoxOptionRepository boxOptionRepository,
            IKoiFishRepository koiFishRepository,
            IPartnerShipmentRepository partnerShipmentRepository,
            IVehicleRepository vehicleRepository,
            IBranchRepository branchRepository)
        {
            _koiDeliDbContext = koiDeliDbContext;
            _accountRepository = accountRepository;
            _boxRepository = boxRepository;
            _boxOptionRepository = boxOptionRepository;
            _koiFishRepository = koiFishRepository;
            _partnerShipmentRepository = partnerShipmentRepository;
            _vehicleRepository = vehicleRepository;
            _branchRepository = branchRepository;
        }


        public IAccountRepository AccountRepository => _accountRepository;
        public IBoxRepository BoxRepository => _boxRepository;
        public IBoxOptionRepository BoxOptionRepository => _boxOptionRepository;    
        public IKoiFishRepository KoiFishRepository => _koiFishRepository;
        public IBranchRepository BranchRepository => _branchRepository;
        public IVehicleRepository VehicleRepository => _vehicleRepository;
        public IPartnerShipmentRepository PartnerShipmentRepository => _partnerShipmentRepository;



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

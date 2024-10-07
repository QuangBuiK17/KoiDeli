

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


        // add vao UnitOfWork
        public UnitOfWork(KoiDeliDbContext koiDeliDbContext,
            IAccountRepository accountRepository,
            IBoxRepository boxRepository,
            IBoxOptionRepository boxOptionRepository,
            IKoiFishRepository koiFishRepository)
        {
            _koiDeliDbContext = koiDeliDbContext;
            _accountRepository = accountRepository;
            _boxRepository = boxRepository;
            _boxOptionRepository = boxOptionRepository;
            _koiFishRepository = koiFishRepository;
        }


        public IAccountRepository AccountRepository => _accountRepository;
        public IBoxRepository BoxRepository => _boxRepository;
        public IBoxOptionRepository BoxOptionRepository => _boxOptionRepository;    
        public IKoiFishRepository KoiFishRepository => _koiFishRepository;



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

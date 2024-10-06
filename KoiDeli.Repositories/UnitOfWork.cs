

using KoiDeli.Repositories.Interfaces;

namespace KoiDeli.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        // khai bao IRepository vao day
        private readonly KoiDeliDbContext _koiDeliDbContext;
        private readonly IAccountRepository _accountRepository;


        // add vao UnitOfWork
        public UnitOfWork(KoiDeliDbContext koiDeliDbContext, IAccountRepository accountRepository)
        {
            _koiDeliDbContext = koiDeliDbContext;
            _accountRepository = accountRepository;
        }


        public IAccountRepository AccountRepository => _accountRepository;


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

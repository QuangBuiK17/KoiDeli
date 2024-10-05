

using KoiDeli.Repositories.Interfaces;

namespace KoiDeli.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KoiDeliDbContext _koiDeliDbContext;

        public UnitOfWork(KoiDeliDbContext koiDeliDbContext)
        {
            _koiDeliDbContext = koiDeliDbContext;
        }





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

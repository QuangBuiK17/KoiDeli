using KoiDeli.Domain.Entities;
using KoiDeli.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly KoiDeliDbContext _dbContext;
        private readonly ICurrentTime _timeService;
        private readonly IClaimsService _claimsService;

        public GenericRepository(KoiDeliDbContext context, ICurrentTime timeService, IClaimsService claimsService)
        {
            _dbSet = context.Set<TEntity>();
            _dbContext = context;
            _timeService = timeService;
            _claimsService = claimsService;
        }

        public Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var result = await query.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                entity.CreationDate = _timeService.GetCurrentTime();
                //var user = await _dbContext.Users.FindAsync(_claimsService.GetCurrentUserId);
                //if (user == null) throw new Exception("This user is no longer existed");
                entity.CreatedBy = _claimsService.GetCurrentUserId;
                var result = await _dbSet.AddAsync(entity);
                //await _dbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.ModificationDate = _timeService.GetCurrentTime();
                entity.ModificationBy = _claimsService.GetCurrentUserId;
            }
            _dbSet.UpdateRange(entities);
            //  await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Update(TEntity entity)
        {
            entity.ModificationDate = _timeService.GetCurrentTime();
            entity.ModificationBy = _claimsService.GetCurrentUserId;
            _dbSet.Update(entity);
            //   await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftRemoveRangeById(List<int> entitiesId) // update hàng loạt cùng 1 trường thì làm y chang
        {
            var entities = await _dbSet.Where(e => entitiesId.Contains(e.Id)).ToListAsync();

            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletionDate = _timeService.GetCurrentTime();
                entity.DeleteBy = _claimsService.GetCurrentUserId;
            }

            _dbContext.UpdateRange(entities);
            return true;
        }

        public async Task<bool> SoftRemoveRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
                entity.DeletionDate = _timeService.GetCurrentTime();
                entity.DeleteBy = _claimsService.GetCurrentUserId;
            }
            _dbSet.UpdateRange(entities);
            //  await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftRemove(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletionDate = _timeService.GetCurrentTime();
            entity.DeleteBy = _claimsService.GetCurrentUserId;
            

            _dbSet.Update(entity);
            // await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreationDate = _timeService.GetCurrentTime();
                entity.CreatedBy = _claimsService.GetCurrentUserId;
            }
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return _dbSet;
        }

        public async Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            // Start by getting the IQueryable of the entity.
            IQueryable<TEntity> query = GetQueryable();

            // Include the related entities if any are specified.
            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            // Apply the filter predicate (search condition).
            query = query.Where(predicate);

            // Execute the query and return the result as a list.
            return await query.ToListAsync();
        }
    }
}

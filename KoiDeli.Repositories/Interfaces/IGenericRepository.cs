using KoiDeli.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KoiDeli.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);

        Task<List<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity> AddAsync(TEntity entity);

        Task<bool> UpdateRange(List<TEntity> entities);

        Task<bool> Update(TEntity entity);

        Task<bool> SoftRemoveRangeById(List<int> entitiesId);

        Task<bool> SoftRemoveRange(List<TEntity> entities);

        Task<bool> SoftRemove(TEntity entity);

        Task<List<TEntity>> AddRangeAsync(List<TEntity> entities);

        IQueryable<TEntity> GetQueryable();

        //IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate);
        //TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        //Task<TEntity?> GetByIdAsync(TKey id);
        //Task<TEntity?> GetByIdCompositeKeyAsync(TKey id1, TKey id2);
        //Task<TEntity> AddAsync(TEntity entity);
        //Task AddRangeAsync(IEnumerable<TEntity> entities);

        //TEntity Update(TEntity entity);
        //TEntity Remove(TKey id);
        //public TEntity RemoveCompositeKey(TKey id1, TKey id2);
        //Task<int> Commit();
        //Task<int> CountAsync();
        //Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        //Task<IEnumerable<TEntity>> GetTopNItems<TKeyProperty>(Expression<Func<TEntity, TKeyProperty>> keySelector, int n);
    }
}

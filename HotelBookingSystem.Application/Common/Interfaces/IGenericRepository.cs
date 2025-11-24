using System.Linq.Expressions;

namespace HotelBookingSystem.Application.Common.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> Query();
}

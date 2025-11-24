using HotelBookingSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBookingSystem.Infrastructure.Data.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<TEntity> _set;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _set = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
        => await _set.FindAsync(new object?[] { id });

    public async Task<List<TEntity>> GetAllAsync()
        => await _set.AsNoTracking().ToListAsync();

    public async Task AddAsync(TEntity entity)
        => await _set.AddAsync(entity);

    public void Update(TEntity entity)
        => _set.Update(entity);

    public void Delete(TEntity entity)
        => _set.Remove(entity);

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        => await _set.AsNoTracking().Where(predicate).ToListAsync();

    public IQueryable<TEntity> Query()
        => _set.AsQueryable();
}

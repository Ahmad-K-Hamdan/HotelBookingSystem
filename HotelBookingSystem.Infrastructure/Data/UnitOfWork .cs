using HotelBookingSystem.Application.Common.Interfaces;

namespace HotelBookingSystem.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync()
        => _dbContext.SaveChangesAsync();
}

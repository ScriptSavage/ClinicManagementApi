using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Helpers;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task RollbackTransaction();
    
}
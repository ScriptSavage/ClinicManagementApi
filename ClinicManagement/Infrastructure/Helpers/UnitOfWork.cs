using System.Transactions;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Helpers;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
       await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel level = IsolationLevel.Serializable)
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task RollbackTransaction()
    {
       await _context.Database.RollbackTransactionAsync();
    }
}
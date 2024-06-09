using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using Tuudio.Domain.Entities;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly TuudioDbContext _context;

    public UnitOfWork(TuudioDbContext context)
    {
        _context = context;

        ClientRepository = new ClientRepository(_context);
    }

    public IClientRepository ClientRepository { get; private set; }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var result = await action();
            var dt = DateTime.Now;

            var entries = _context.ChangeTracker.Entries()
                .Where(entry => entry.Entity is DbObject && (entry.State == EntityState.Added || entry.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                entry.Property("UpdatedDate").CurrentValue = dt;

                if (entry.State == EntityState.Added)
                    entry.Property("CreatedDate").CurrentValue = dt;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return result;
        }
        catch (Exception)
        {
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }
}

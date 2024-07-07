using Microsoft.EntityFrameworkCore;
using Tuudio.Domain.Entities;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly TuudioDbContext _context;

    public UnitOfWork(TuudioDbContext context, IClientRepository clientRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        ClientRepository = clientRepository;
    }

    public IClientRepository ClientRepository { get; private set; }

    public void Dispose() => _context.Dispose();

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        //if (entity is not DbObject dbEntity)
        //    throw new ArgumentException($"Entity must inherit from {nameof(DbObject)}.");

        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var result = await action();
            var datetime = DateTime.Now;

            var entries = _context.ChangeTracker.Entries()
                .Where(entry => entry.Entity is DbObject && (entry.State == EntityState.Added || entry.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                entry.Property("UpdatedDate").CurrentValue = datetime;

                if (entry.State == EntityState.Added)
                    entry.Property("CreatedDate").CurrentValue = datetime;
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

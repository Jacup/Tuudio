using Microsoft.EntityFrameworkCore;
using Tuudio.Domain.Entities;
using Tuudio.Domain.Exceptions;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly TuudioDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(TuudioDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    protected DbSet<T> DbSet => _dbSet;

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new EntityNotFoundException<T>(id);

        _dbSet.Remove(entity);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbSet
            .Where(entity => ids.Contains((Guid)typeof(T).GetProperty("Id")!.GetValue(entity)))
            .ToListAsync();
    }

    public virtual async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is not DbObject dbEntity)
            throw new ArgumentException($"Entity must inherit from {nameof(DbObject)}.");

        var current = await _dbSet.FindAsync(dbEntity.Id) ?? throw new EntityNotFoundException<T>(dbEntity.Id);

        _dbSet.Entry(current).CurrentValues.SetValues(entity);
    }

    public virtual async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}

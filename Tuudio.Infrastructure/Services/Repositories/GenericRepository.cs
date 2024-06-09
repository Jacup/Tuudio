using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tuudio.Domain.Entities;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly TuudioDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(TuudioDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    protected DbSet<T> DbSet => _dbSet;

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new Exception();

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

    public virtual async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        if (entity is not DbObject bdEntity)
            throw new Exception();

        var current = await _dbSet.FindAsync(bdEntity.Id) ?? throw new Exception();

        _dbSet.Entry(current).CurrentValues.SetValues(entity);
    }
}

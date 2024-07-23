namespace Tuudio.Infrastructure.Services.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task DeleteAsync(Guid id);

    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids);

    Task InsertAsync(T entity);

    Task UpdateAsync(T entity);
}

using Microsoft.EntityFrameworkCore;
using Tuudio.Domain.Entities.Entries;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class EntryRepository(TuudioDbContext context) : GenericRepository<Entry>(context), IEntryRepository
{
    public override async Task<IEnumerable<Entry>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        if (ids == null || !ids.Any())
        {
            return [];
        }

        return await DbSet
            .Where(entity => ids.Contains(entity.Id))
            .ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class PassRepository(TuudioDbContext context) : GenericRepository<Pass>(context), IPassRepository
{
    public override async Task<IEnumerable<Pass>> GetByIdsAsync(IEnumerable<Guid> ids)
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

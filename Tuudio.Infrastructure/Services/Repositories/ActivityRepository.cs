using Microsoft.EntityFrameworkCore;
using Tuudio.Domain.Entities.Activities;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class ActivityRepository(TuudioDbContext context) : GenericRepository<Activity>(context), IActivityRepository
{
    public async override Task<IEnumerable<Activity>> GetByIdsAsync(IEnumerable<Guid> ids)
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

using Microsoft.EntityFrameworkCore;
using Tuudio.Domain.Entities.PassTemplates;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class PassTemplateRepository(TuudioDbContext context) : GenericRepository<PassTemplate>(context), IPassTemplateRepository
{

    public async override Task<IEnumerable<PassTemplate>> GetAllAsync()
    {
        return await DbSet
            .Include(e => e.Activities)
            .ToListAsync();
    }

    public async override Task<PassTemplate?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(e => e.Activities)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async override Task<IEnumerable<PassTemplate>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        if (ids == null || !ids.Any())
        {
            return [];
        }

        return await DbSet
            .Where(entity => ids.Contains(entity.Id))
            .Include(e => e.Activities)
            .ToListAsync();
    }
}

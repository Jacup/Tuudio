using Tuudio.Domain.Entities.Activities;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class ActivityRepository(TuudioDbContext context) : GenericRepository<Activity>(context), IActivityRepository
{
}

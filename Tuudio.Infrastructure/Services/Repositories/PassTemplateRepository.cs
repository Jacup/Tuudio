using Tuudio.Domain.Entities.PassTemplates;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class PassTemplateRepository(TuudioDbContext context) : GenericRepository<PassTemplate>(context), IPassTemplateRepository
{
}

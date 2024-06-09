using Microsoft.Extensions.Logging;
using Tuudio.Domain.Entities.People;
using Tuudio.Infrastructure.Data;
using Tuudio.Infrastructure.Services.Interfaces;

namespace Tuudio.Infrastructure.Services.Repositories;

public class ClientRepository(TuudioDbContext context) : GenericRepository<Client>(context), IClientRepository
{
}

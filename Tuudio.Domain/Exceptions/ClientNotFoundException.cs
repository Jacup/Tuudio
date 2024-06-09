using Tuudio.Domain.Entities.People;

namespace Tuudio.Domain.Exceptions;

public class ClientNotFoundException(Guid clientId) : EntityNotFoundException<Client>(clientId)
{
}

namespace Tuudio.Domain.Exceptions;

public class ClientNotFoundException(Guid clientId) : Exception($"Client with Id '{clientId}' not found.")
{
}

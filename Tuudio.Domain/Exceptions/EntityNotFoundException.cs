namespace Tuudio.Domain.Exceptions;

public class EntityNotFoundException<T> : Exception
{
    public EntityNotFoundException(Guid entityId)
        : base($"Entity of type '{typeof(T).Name}' with ID = '{entityId}' not found")
    {
        EntityId = entityId;
    }

    public Guid EntityId { get; private set; }
}

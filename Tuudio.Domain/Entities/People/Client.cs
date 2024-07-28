using Tuudio.Domain.Entities.Passes;

namespace Tuudio.Domain.Entities.People;

public class Client : Person
{
    public ICollection<Pass> Passes { get; set; } = [];
}

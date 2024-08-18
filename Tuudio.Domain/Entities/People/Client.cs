using Tuudio.Domain.Entities.Entries;
using Tuudio.Domain.Entities.Passes;

namespace Tuudio.Domain.Entities.People;

public class Client : Person
{
    public ICollection<Pass> Passes { get; set; } = [];

    public ICollection<Entry> Entries { get; set; } = [];
}

using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Entities.People;

namespace Tuudio.Domain.Entities.Entries;

public class Entry : DbObject
{
    public required DateTime EntryDate { get; set; }

    public string? Note { get; set; }

    public Guid ClientId { get; set; }
    
    public Client Client { get; set; }

    public Guid? PassId { get; set; }

    public Pass? Pass { get; set; }
}

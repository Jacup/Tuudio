using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Entities.People;

namespace Tuudio.Domain.Entities.Entries;

public class Entry : DbObject
{
    public required DateTime EntryDate { get; set; }

    public string? Note { get; set; }
    
    public required Guid ActivityId { get; set; }
    
    public Activity Activity { get; set; }

    public required Guid ClientId { get; set; }
    
    public Client Client { get; set; }

    public Guid? PassId { get; set; }

    public Pass? Pass { get; set; }

    public override string ToString() => $"Entry on {EntryDate:dd.MM.yyyy HH:mm}";
}

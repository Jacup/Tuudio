using Tuudio.Domain.Entities.Activities;

namespace Tuudio.Domain.Entities.Passes;

public class PassTemplate : DbObject
{
    public required string Name { get; set; }

    public Price Price { get; set; }

    public Duration Duration { get; set; }

    public int Entries { get; set; }

    public string? Description { get; set; }

    public ICollection<Activity> Activities { get; set; } = [];

    public ICollection<Pass> Passes { get; set; } = [];
}

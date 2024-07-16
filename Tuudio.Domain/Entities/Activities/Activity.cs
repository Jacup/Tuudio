namespace Tuudio.Domain.Entities.Activities;

public class Activity : DbObject
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}

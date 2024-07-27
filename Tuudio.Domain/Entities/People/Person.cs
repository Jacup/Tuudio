namespace Tuudio.Domain.Entities.People;

public abstract class Person : DbObject
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public override string? ToString() => $"{FirstName} {LastName}";
}

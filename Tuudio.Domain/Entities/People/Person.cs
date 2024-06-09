namespace Tuudio.Domain.Entities.People;

public abstract class Person : DbObject
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}

namespace Tuudio.DTOs;

public abstract record DbObjectDto
{
    public required Guid Id { get; set; }

    public required DateTime CreatedDate { get; set; }

    public required DateTime UpdatedDate { get; set; }
}

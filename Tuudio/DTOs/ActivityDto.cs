namespace Tuudio.DTOs;

public record ActivityDto
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<Guid> PassTemplates { get; set; } = [];

    public ICollection<Guid> Entries { get; set; } = [];
}

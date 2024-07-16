namespace Tuudio.DTOs;

public record ActivityDetailedDto : DbObjectDto
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}

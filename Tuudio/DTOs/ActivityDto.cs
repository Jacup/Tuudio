namespace Tuudio.DTOs;

public record ActivityDto
{
    public required string Name { get; set; }

    public string? Description { get; set; }
}

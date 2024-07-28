using Tuudio.Domain.Enums;

namespace Tuudio.DTOs;

public record PassTemplateDetailedDto : DbObjectDto
{
    public required string Name { get; set; }

    public decimal Price_Amount { get; set; }

    public Period Price_Period { get; set; }

    public int Duration_Amount { get; set; }

    public Period Duration_Period { get; set; }

    public int Entries { get; set; }

    public string? Description { get; set; }

    public ICollection<Guid> Activities { get; set; } = [];
}

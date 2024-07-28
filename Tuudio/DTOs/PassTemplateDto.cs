using Tuudio.Domain.Enums;

namespace Tuudio.DTOs;

public record PassTemplateDto
{
    public required string Name { get; set; }

    public decimal Price_Amount { get; set; }

    public required Period Price_Period { get; set; }

    public required int Duration_Amount { get; set; }

    public required Period Duration_Period { get; set; }

    public required int Entries { get; set; }

    public string? Description { get; set; }

    public ICollection<Guid> Activities { get; set; } = [];
}

using Tuudio.Domain.Entities.Entries;

namespace Tuudio.DTOs;

public record PassDetailedDto : DbObjectDto
{
    public required DateOnly FromDate { get; set; }

    public required DateOnly ToDate { get; set; }

    public required Guid ClientId { get; set; }

    public required Guid PassTemplateId { get; set; }

    public ICollection<Guid> Entries { get; set; } = [];
}

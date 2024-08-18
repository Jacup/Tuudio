using Tuudio.Domain.Entities.Entries;
using Tuudio.DTOs.Interfaces;

namespace Tuudio.DTOs;

public record EntryDetailedDto : DbObjectDto, IDetailedDto<Entry>
{
    public required DateTime EntryDate { get; set; }

    public string? Note { get; set; }
 
    public required Guid ActivityId { get; set; }

    public required Guid ClientId { get; set; }

    public Guid? PassId { get; set; }
}

using Tuudio.Domain.Entities.Entries;

namespace Tuudio.DTOs.People.Detailed;

public record ClientDetailedDto : PersonDetailedDto
{
    public ICollection<Guid> Passes { get; set; } = [];

    public ICollection<Guid> Entries { get; set; } = [];

}

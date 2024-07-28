namespace Tuudio.DTOs.People.Detailed;

public record ClientDetailedDto : PersonDetailedDto
{
    public ICollection<Guid> Passes { get; set; } = [];
}

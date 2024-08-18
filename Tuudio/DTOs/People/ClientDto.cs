namespace Tuudio.DTOs.People;

public record ClientDto : PersonDto
{
    public ICollection<Guid> Passes { get; set; } = [];
    
    public ICollection<Guid> Entries { get; set; } = [];
}

using Tuudio.Domain.Entities.People;

namespace Tuudio.Domain.Entities.Passes;

public class Pass : DbObject
{
    public required DateOnly FromDate { get; set; }

    public required DateOnly ToDate { get; set; }

    public required Guid ClientId { get; set; }

    public Client Client { get; set; }
    
    public required Guid PassTemplateId { get; set; }

    public PassTemplate PassTemplate { get; set; }

    public override string? ToString() => $"{PassTemplate.Name}: {FromDate.ToShortDateString} -> {ToDate.ToShortDateString}";
}

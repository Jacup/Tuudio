using Tuudio.Domain.Enums;

namespace Tuudio.Domain.Entities.PassTemplates;

public class Duration
{
    public int Amount { get; set; }

    public Period Period { get; set; }
}
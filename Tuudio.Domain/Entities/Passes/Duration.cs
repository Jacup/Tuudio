using Tuudio.Domain.Enums;

namespace Tuudio.Domain.Entities.Passes;

public class Duration
{
    public int Amount { get; set; }

    public Period Period { get; set; }
}
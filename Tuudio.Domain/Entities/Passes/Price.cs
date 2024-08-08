using Tuudio.Domain.Enums;

namespace Tuudio.Domain.Entities.Passes;

public class Price
{
    public decimal Amount { get; set; }

    public Period Period { get; set; }
}
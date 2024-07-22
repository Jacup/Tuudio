using Tuudio.Domain.Enums;

namespace Tuudio.Domain.Entities.PassTemplates;

public class Price
{
    public decimal Amount { get; set; }

    public Period Period { get; set; }
}
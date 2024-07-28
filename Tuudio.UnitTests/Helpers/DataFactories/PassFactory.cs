using Tuudio.Domain.Entities.Passes;
using Tuudio.DTOs;

namespace Tuudio.UnitTests.Helpers.DataFactories;

internal class PassFactory
{
    internal static DateOnly FromDate = DateOnly.ParseExact("01-01-2024", "dd-MM-yyyy");
    internal static DateOnly ToDate = DateOnly.ParseExact("01-04-2024", "dd-MM-yyyy");

    internal static Pass GetPass() => GetPass(new Guid("00000000-0000-0000-0003-000000000001"));

    internal static Pass GetPass(Guid id) => new()
    {
        Id = id,

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        FromDate = FromDate,
        ToDate = ToDate,

        ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
        PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
    };

    internal static PassDetailedDto GetPassDetailedDto() => new()
    {
        Id = new Guid("00000000-0000-0000-0003-000000000001"),

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        FromDate = FromDate,
        ToDate = ToDate,

        ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
        PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
    };

    internal static PassDto GetPassDto() => new()
    {
        FromDate = FromDate,
        ToDate = ToDate,

        ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
        PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
    };
}

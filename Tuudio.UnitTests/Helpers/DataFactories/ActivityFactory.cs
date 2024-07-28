using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Enums;
using Tuudio.DTOs;

namespace Tuudio.UnitTests.Helpers.DataFactories;

internal class ActivityFactory
{
    internal static Activity GetActivity() => GetActivity(new Guid("00000000-0000-0000-0001-000000000001"));

    internal static Activity GetActivity(Guid id) => new()
    {
        Id = id,
        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        Name = "EMS Training",
        Description = "Basic 60min EMS training",

        PassTemplates =
        [
            new()
            {
                Id = new Guid("00000000-0000-0000-0002-000000000001"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddDays(1),

                Name = "Monthly yoga pass",
                Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
                Entries = 0,

                Price = new Price() { Amount = 50, Period = Period.Month, },
                Duration = new Duration() { Amount = 3, Period = Period.Month },
            },
            new()
            {
                Id = new Guid("00000000-0000-0000-0002-000000000002"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddDays(1),

                Name = "Monthly yoga pass",
                Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
                Entries = 0,

                Price = new Price() { Amount = 50, Period = Period.Month, },
                Duration = new Duration() { Amount = 3, Period = Period.Month },
            },
        ],
    };

    internal static ActivityDetailedDto GetActivityDetailedDto() => new()
    {
        Id = Guid.Parse("00000000-0000-0000-0001-000000000001"),
        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        Name = "EMS Training",
        Description = "Basic 60min EMS training",

        PassTemplates =
        [
           new Guid("00000000-0000-0000-0002-000000000001"),
           new Guid("00000000-0000-0000-0002-000000000002"),
        ],
    };

    internal static ActivityDto GetActivityDto() => new()
    {
        Name = "EMS Training",
        Description = "Basic 60min EMS training",

        PassTemplates =
        [
           new Guid("00000000-0000-0000-0002-000000000001"),
           new Guid("00000000-0000-0000-0002-000000000002"),
        ],
    };
}

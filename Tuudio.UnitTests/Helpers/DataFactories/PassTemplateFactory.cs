using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Enums;
using Tuudio.DTOs;

namespace Tuudio.UnitTests.Helpers.DataFactories;

internal class PassTemplateFactory
{
    internal static PassTemplate GetPassTemplate() => GetPassTemplate(new Guid("00000000-0000-0000-0002-000000000001"));

    internal static PassTemplate GetPassTemplate(Guid id) => new()
    {
        Id = id,
        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        Name = "Monthly yoga pass",
        Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
        Entries = 0,

        Price = new Price() { Amount = 50, Period = Period.Month, },
        Duration = new Duration() { Amount = 3, Period = Period.Month },

        Activities =
        [
            new Activity()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000001"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddDays(1),

                Name = "EMS Training",
                Description = "Basic 60min EMS training",
            },
            new Activity()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000002"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddDays(1),

                Name = "EMS Training",
                Description = "Basic 60min EMS training",
            },
        ],

        Passes =
        [
            new Pass()
            {
                Id = new Guid("00000000-0000-0000-0003-000000000001"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddDays(1),
                FromDate = PassFactory.FromDate,
                ToDate = PassFactory.ToDate,
                ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
            },
            new Pass()
            {
                Id = new Guid("00000000-0000-0000-0003-000000000002"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now.AddDays(1),
                FromDate = PassFactory.FromDate,
                ToDate = PassFactory.ToDate,
                ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
            }
        ]
    };

    internal static PassTemplateDetailedDto GetPassTemplateDetailedDto() => new()
    {
        Id = new Guid("00000000-0000-0000-0002-000000000001"),
        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        Name = "Monthly yoga pass",
        Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
        Entries = 0,

        Price_Amount = 50,
        Price_Period = Period.Month,

        Duration_Amount = 3,
        Duration_Period = Period.Month,

        Activities =
        [
            new Guid("00000000-0000-0000-0001-000000000001"),
            new Guid("00000000-0000-0000-0001-000000000002"),
        ],
        Passes =
        [
            new Guid("00000000-0000-0000-0003-000000000001"),
            new Guid("00000000-0000-0000-0003-000000000001")
        ]
    };

    internal static PassTemplateDto GetPassTemplateDto() => new()
    {
        Name = "Monthly yoga pass",
        Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
        Entries = 0,

        Price_Amount = 50,
        Price_Period = Period.Month,

        Duration_Amount = 3,
        Duration_Period = Period.Month,

        Activities =
        [
            new Guid("00000000-0000-0000-0001-000000000001"),
            new Guid("00000000-0000-0000-0001-000000000002"),
        ],
        Passes =
        [
            new Guid("00000000-0000-0000-0003-000000000001"),
            new Guid("00000000-0000-0000-0003-000000000001")
        ]
    };
}
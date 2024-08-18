using Tuudio.Domain.Entities.Entries;
using Tuudio.DTOs;

namespace Tuudio.UnitTests.Helpers.DataFactories;

internal static class EntryFactory
{
    public static EntryDetailedDto GetDetailedDto() => new()
    {
        Id = new Guid("00000000-0000-0000-0004-000000000001"),

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        EntryDate = DateTime.Now,
        Note = "Some note on 01 entry",

        ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
        PassId = new Guid("00000000-0000-0000-0003-000000000001"),
        ActivityId = new Guid("00000000-0000-0000-0001-000000000001"),
    };

    public static EntryDto GetDto() => new()
    {
        EntryDate = DateTime.Now,
        Note = "Some note on 01 entry",

        ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
        PassId = new Guid("00000000-0000-0000-0003-000000000001"),
        ActivityId = new Guid("00000000-0000-0000-0001-000000000001"),
    };

    public static Entry GetSingle() => GetSingle(new Guid("00000000-0000-0000-0004-000000000001"));

    public static Entry GetSingle(Guid id) => new()
    {
        Id = id,

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        EntryDate = DateTime.Now,
        Note = "Some note on 01 entry",

        ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
        PassId = new Guid("00000000-0000-0000-0003-000000000001"),
        ActivityId = new Guid("00000000-0000-0000-0001-000000000001"),
    };
}
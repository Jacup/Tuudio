using Tuudio.Domain.Entities.Passes;
using Tuudio.Domain.Entities.People;
using Tuudio.DTOs.People;
using Tuudio.DTOs.People.Detailed;

namespace Tuudio.UnitTests.Helpers.DataFactories;

internal class ClientFactory
{
    internal static Client GetClient() => GetClient(new Guid("00000000-0000-0000-0000-000000000001"));

    internal static Client GetClient(Guid id) => new()
    {
        Id = id,

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        FirstName = "John",
        LastName = "Doe",
        Email = "example@site.com",
        PhoneNumber = "+48123456789",

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
                PassTemplateId = new Guid("00000000-0000-0000-0002-000000000002"),
            }
        ]
    };

    internal static ClientDetailedDto GetClientDetailedDto() => new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        FirstName = "John",
        LastName = "Doe",
        Email = "example@site.com",
        PhoneNumber = "+48123456789",

        Passes =
        [
            new Guid("00000000-0000-0000-0003-000000000001"),
            new Guid("00000000-0000-0000-0003-000000000002")
        ]
    };

    internal static ClientDto GetClientDto() => new()
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "example@site.com",
        PhoneNumber = "+48123456789",

        Passes =
        [
            new Guid("00000000-0000-0000-0003-000000000001"),
            new Guid("00000000-0000-0000-0003-000000000002")
        ]
    };
}

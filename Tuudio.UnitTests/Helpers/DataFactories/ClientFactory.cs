using Tuudio.Domain.Entities.People;
using Tuudio.DTOs.People;
using Tuudio.DTOs.People.Detailed;

namespace Tuudio.UnitTests.Helpers.DataFactories;

internal class ClientFactory
{
    internal static Client GetClient() => new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        FirstName = "John",
        LastName = "Doe",
        Email = "example@site.com",
        PhoneNumber = "+48123456789"
    };

    internal static ClientDetailedDto GetClientDetailedDto() => new()
    {
        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),

        CreatedDate = DateTime.Now,
        UpdatedDate = DateTime.Now.AddDays(1),

        FirstName = "John",
        LastName = "Doe",
        Email = "example@site.com",
        PhoneNumber = "+48123456789"
    };

    internal static ClientDto GetClientDto() => new()
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "example@site.com",
        PhoneNumber = "+48123456789"
    };
}

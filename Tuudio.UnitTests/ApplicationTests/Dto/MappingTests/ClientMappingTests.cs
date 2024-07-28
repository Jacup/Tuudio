using Mapster;
using NUnit.Framework.Internal;
using Tuudio.Domain.Entities.People;
using Tuudio.DTOs.People.Detailed;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;

[TestFixture]
public class ClientMappingTests
{
    [Test]
    public void Adapt_ClientToDetailedClientDto_AllValuesShouldBeEqual()
    {
        var client = ClientFactory.GetClient();
        
        var clientDetailedDto = client.Adapt<ClientDetailedDto>();

        clientDetailedDto.GetType().ShouldBeEquivalentTo(typeof(ClientDetailedDto));

        clientDetailedDto.Id.ShouldBeEquivalentTo(client.Id);
        clientDetailedDto.CreatedDate.ShouldBeEquivalentTo(client.CreatedDate);
        clientDetailedDto.UpdatedDate.ShouldBeEquivalentTo(client.UpdatedDate);
        clientDetailedDto.FirstName.ShouldBeEquivalentTo(client.FirstName);
        clientDetailedDto.LastName.ShouldBeEquivalentTo(client.LastName);
        clientDetailedDto.Email.ShouldBeEquivalentTo(client.Email);
        clientDetailedDto.PhoneNumber.ShouldBeEquivalentTo(client.PhoneNumber);
    }

    [Test]
    public void Adapt_ClientDtoToClient_AllValuesShouldBeEqual()
    {
        var clientDto = ClientFactory.GetClientDto();

        var client = clientDto.Adapt<Client>();

        client.GetType().ShouldBeEquivalentTo(typeof(Client));

        client.FirstName.ShouldBeEquivalentTo(clientDto.FirstName);
        client.LastName.ShouldBeEquivalentTo(clientDto.LastName);
        client.Email.ShouldBeEquivalentTo(clientDto.Email);
        client.PhoneNumber.ShouldBeEquivalentTo(clientDto.PhoneNumber);
    }
}

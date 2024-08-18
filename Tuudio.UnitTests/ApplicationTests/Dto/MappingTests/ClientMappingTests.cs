using Mapster;
using NUnit.Framework.Internal;
using Tuudio.Domain.Entities.People;
using Tuudio.DTOs.People.Detailed;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;

[TestFixture]
internal class ClientMappingTests : MappingTestBase
{
    [Test]
    public void Adapt_ClientToDetailedClientDto_TypeShouldBeEqualToAdapted()
    {
        var client = ClientFactory.GetClient();

        var clientDetailedDto = client.Adapt<ClientDetailedDto>();

        clientDetailedDto.GetType().ShouldBeEquivalentTo(typeof(ClientDetailedDto));
    }

    [Test]
    public void Adapt_ClientToDetailedClientDto_AllValuesShouldBeEqual()
    {
        var client = ClientFactory.GetClient();

        var clientDetailedDto = client.Adapt<ClientDetailedDto>();

        clientDetailedDto.Id.ShouldBeEquivalentTo(client.Id);
        clientDetailedDto.CreatedDate.ShouldBeEquivalentTo(client.CreatedDate);
        clientDetailedDto.UpdatedDate.ShouldBeEquivalentTo(client.UpdatedDate);
        clientDetailedDto.FirstName.ShouldBeEquivalentTo(client.FirstName);
        clientDetailedDto.LastName.ShouldBeEquivalentTo(client.LastName);
        clientDetailedDto.Email.ShouldBeEquivalentTo(client.Email);
        clientDetailedDto.PhoneNumber.ShouldBeEquivalentTo(client.PhoneNumber);
        clientDetailedDto.Passes.ShouldBeEquivalentTo(client.Passes.Select(p => p.Id).ToList());
        clientDetailedDto.Entries.ShouldBeEquivalentTo(client.Entries.Select(p => p.Id).ToList());
    }

    [Test]
    public void Adapt_ClientDtoToClient_ObjectTypeShouldBeEqualToAdapted()
    {
        var clientDto = ClientFactory.GetClientDto();

        var client = clientDto.Adapt<Client>();

        client.GetType().ShouldBeEquivalentTo(typeof(Client));
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

    [Test]
    public void Adapt_ClientDtoToClient_ShouldMapPassTemplatesIdsAsEmptyPassTemplates()
    {
        var clientDto = ClientFactory.GetClientDto();

        var result = clientDto.Adapt<Client>();

        result.Passes.ShouldNotBeNull();
        result.Passes.ShouldBeEmpty();
    }

    [Test]
    public void Adapt_ClientDtoToClient_ShouldMapEntriesIdsAsEmptyEntries()
    {
        var clientDto = ClientFactory.GetClientDto();

        var result = clientDto.Adapt<Client>();

        result.Entries.ShouldNotBeNull();
        result.Entries.ShouldBeEmpty();
    }
}

using Tuudio.Domain.Entities.People;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.DomainTests.ModelsTests;

[TestFixture]
public class ClientTests
{
    [Test]
    public void Client_IsChildOfPerson_ShouldReturnTrue()
    {
        bool isSubclass = typeof(Client).IsSubclassOf(typeof(Person));

        Assert.That(isSubclass, Is.True);
    }

    [Test]
    public void ToString_ValidDataProvided_ShouldBeEqualToFirstAndLastName()
    {
        var client = ClientFactory.GetClient();

        client.ToString().ShouldBeEquivalentTo($"{client.FirstName} {client.LastName}");
    }
}

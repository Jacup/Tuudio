using Tuudio.Domain.Entities.People;

namespace Tuudio.UnitTests.DomainTests.ModelsTests;

[TestFixture]
public class ClientTests
{
    [Test]
    public void ClientTest_IsChildOfPerson_ShouldReturnTrue()
    {
        bool isSubclass = typeof(Client).IsSubclassOf(typeof(Person));

        Assert.That(isSubclass, Is.True);
    }
}

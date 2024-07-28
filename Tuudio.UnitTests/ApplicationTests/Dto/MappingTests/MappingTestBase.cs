using Tuudio.Services.Mapping;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;

internal class MappingTestBase
{
    [SetUp]
    public void SetUp()
    {
        MapsterConfiguration.Configure();
    }
}

using Tuudio.Services.Mapping;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;

public class MappingTestBase
{
    [SetUp]
    public void SetUp()
    {
        MapsterConfiguration.Configure();
    }
}

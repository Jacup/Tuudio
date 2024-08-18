using Mapster;
using Tuudio.Domain.Entities.Activities;
using Tuudio.DTOs;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;

internal class ActivityMappingTests : MappingTestBase
{
    [Test]
    public void Adapt_ActivityToDetailedActivityDto_TypeShouldBeEqualToAdapted()
    {
        var baseEntity = ActivityFactory.GetActivity();

        var result = baseEntity.Adapt<ActivityDetailedDto>();

        result.GetType().ShouldBeEquivalentTo(typeof(ActivityDetailedDto));
    }

    [Test]
    public void Adapt_ActivityToDetailedActivityDto_AllValuesShouldBeEqual()
    {
        var baseEntity = ActivityFactory.GetActivity();

        var result = baseEntity.Adapt<ActivityDetailedDto>();

        result.Id.ShouldBeEquivalentTo(baseEntity.Id);
        result.CreatedDate.ShouldBeEquivalentTo(baseEntity.CreatedDate);
        result.UpdatedDate.ShouldBeEquivalentTo(baseEntity.UpdatedDate);
        result.Name.ShouldBeEquivalentTo(baseEntity.Name);
        result.Description.ShouldBeEquivalentTo(baseEntity.Description);
        result.PassTemplates.ShouldBeEquivalentTo(baseEntity.PassTemplates.Select(passtemplate => passtemplate.Id).ToList());
        result.Entries.ShouldBeEquivalentTo(baseEntity.Entries.Select(entry => entry.Id).ToList());
    }

    [Test]
    public void Adapt_ActivityDtoToActivity_ObjectTypeShouldBeEqualToAdapted()
    {
        var baseDto = ActivityFactory.GetActivityDto();

        var result = baseDto.Adapt<Activity>();

        result.GetType().ShouldBeEquivalentTo(typeof(Activity));
    }

    [Test]
    public void Adapt_ActivityDtoToActivity_AllValuesShouldBeEqual()
    {
        var baseDto = ActivityFactory.GetActivityDto();

        var result = baseDto.Adapt<Activity>();

        result.Name.ShouldBeEquivalentTo(baseDto.Name);
        result.Description.ShouldBeEquivalentTo(baseDto.Description);
    }

    [Test]
    public void Adapt_ActivityDtoToActivity_ShouldMapPassTemplatesIdsAsEmptyPassTemplates()
    {
        var baseDto = ActivityFactory.GetActivityDto();

        var result = baseDto.Adapt<Activity>();

        result.PassTemplates.ShouldNotBeNull();
        result.PassTemplates.ShouldBeEmpty();
    }
}

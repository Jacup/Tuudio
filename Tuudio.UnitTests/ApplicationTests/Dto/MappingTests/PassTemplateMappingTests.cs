using Mapster;
using Tuudio.Domain.Entities.Passes;
using Tuudio.DTOs;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;

internal class PassTemplateMappingTests : MappingTestBase
{
    [Test]
    public void Adapt_PassTemplateToDetailedPassTemplateDto_TypeShouldBeEqualToAdapted()
    {
        var baseEntity = PassTemplateFactory.GetPassTemplate();

        var result = baseEntity.Adapt<PassTemplateDetailedDto>();

        result.GetType().ShouldBeEquivalentTo(typeof(PassTemplateDetailedDto));
    }

    [Test]
    public void Adapt_PassTemplateToDetailedPassTemplateDto_AllValuesShouldBeEqual()
    {
        var baseEntity = PassTemplateFactory.GetPassTemplate();

        var result = baseEntity.Adapt<PassTemplateDetailedDto>();

        result.Id.ShouldBeEquivalentTo(baseEntity.Id);
        result.CreatedDate.ShouldBeEquivalentTo(baseEntity.CreatedDate);
        result.UpdatedDate.ShouldBeEquivalentTo(baseEntity.UpdatedDate);

        result.Name.ShouldBeEquivalentTo(baseEntity.Name);
        result.Description.ShouldBeEquivalentTo(baseEntity.Description);
        result.Price_Amount.ShouldBeEquivalentTo(baseEntity.Price.Amount);
        result.Price_Period.ShouldBeEquivalentTo(baseEntity.Price.Period);
        result.Duration_Amount.ShouldBeEquivalentTo(baseEntity.Duration.Amount);
        result.Duration_Period.ShouldBeEquivalentTo(baseEntity.Duration.Period);
        result.Activities.ShouldBeEquivalentTo(baseEntity.Activities.Select(passtemplate => passtemplate.Id).ToList());
    }

    [Test]
    public void Adapt_PassTemplateDtoToPassTemplate_ObjectTypeShouldBeEqualToAdapted()
    {
        var baseDto = PassTemplateFactory.GetPassTemplateDto();

        var result = baseDto.Adapt<PassTemplate>();

        result.GetType().ShouldBeEquivalentTo(typeof(PassTemplate));
    }

    [Test]
    public void Adapt_PassTemplateDtoToPassTemplate_AllValuesShouldBeEqual()
    {
        var baseDto = PassTemplateFactory.GetPassTemplateDto();

        var result = baseDto.Adapt<PassTemplate>();

        result.Name.ShouldBeEquivalentTo(baseDto.Name);
        result.Description.ShouldBeEquivalentTo(baseDto.Description);
        result.Price.Amount.ShouldBeEquivalentTo(baseDto.Price_Amount);
        result.Price.Period.ShouldBeEquivalentTo(baseDto.Price_Period);
        result.Duration.Amount.ShouldBeEquivalentTo(baseDto.Duration_Amount);
        result.Duration.Period.ShouldBeEquivalentTo(baseDto.Duration_Period);
    }

    [Test]
    public void Adapt_PassTemplateDtoToPassTemplate_ShouldMapPassTemplatesIdsAsEmptyPassTemplates()
    {
        var baseDto = PassTemplateFactory.GetPassTemplateDto();

        var result = baseDto.Adapt<PassTemplate>();

        result.Activities.ShouldNotBeNull();
        result.Activities.ShouldBeEmpty();
    }
}

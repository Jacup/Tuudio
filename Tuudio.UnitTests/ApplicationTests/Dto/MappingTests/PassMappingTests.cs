using Mapster;
using Tuudio.Domain.Entities.Passes;
using Tuudio.DTOs;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;
internal class PassMappingTests : MappingTestBase
{
    [Test]
    public void Adapt_PassToDetailedPassDto_TypeShouldBeEqualToAdapted()
    {
        var pass = PassFactory.GetPass();

        var passDetailedDto = pass.Adapt<PassDetailedDto>();

        passDetailedDto.GetType().ShouldBeEquivalentTo(typeof(PassDetailedDto));
    }

    [Test]
    public void Adapt_PassToDetailedPassDto_AllValuesShouldBeEqual()
    {
        var pass = PassFactory.GetPass();

        var passDetailedDto = pass.Adapt<PassDetailedDto>();

        passDetailedDto.Id.ShouldBeEquivalentTo(pass.Id);
        passDetailedDto.CreatedDate.ShouldBeEquivalentTo(pass.CreatedDate);
        
        passDetailedDto.FromDate.ShouldBeEquivalentTo(pass.FromDate);
        passDetailedDto.ToDate.ShouldBeEquivalentTo(pass.ToDate);
        passDetailedDto.ClientId.ShouldBeEquivalentTo(pass.ClientId);
        passDetailedDto.PassTemplateId.ShouldBeEquivalentTo(pass.PassTemplateId);
    }

    [Test]
    public void Adapt_PassDtoToPass_ObjectTypeShouldBeEqualToAdapted()
    {
        var passDto = PassFactory.GetPassDto();

        var pass = passDto.Adapt<Pass>();

        pass.GetType().ShouldBeEquivalentTo(typeof(Pass));
    }

    [Test]
    public void Adapt_PassDtoToPass_AllValuesShouldBeEqual()
    {
        var passDto = PassFactory.GetPassDto();

        var pass = passDto.Adapt<Pass>();

        pass.FromDate.ShouldBeEquivalentTo(passDto.FromDate);
        pass.ToDate.ShouldBeEquivalentTo(passDto.ToDate);
        pass.ClientId.ShouldBeEquivalentTo(passDto.ClientId);
        pass.PassTemplateId.ShouldBeEquivalentTo(passDto.PassTemplateId);
    }
}

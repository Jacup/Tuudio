using Mapster;
using Tuudio.Domain.Entities.Entries;
using Tuudio.DTOs;
using Tuudio.UnitTests.Helpers.DataFactories;

namespace Tuudio.UnitTests.ApplicationTests.Dto.MappingTests;

public class EntryMappingTests
{
    [Test]
    public void Adapt_EntryToDetailedDto_TypeShouldBeEqualToAdapted()
    {
        var entry = EntryFactory.GetSingle();

        var detailedDto = entry.Adapt<EntryDetailedDto>();

        detailedDto.GetType().ShouldBe(typeof(EntryDetailedDto));
    }

    [Test]
    public void Adapt_EntryToDetailedDto_AllValuesShouldBeEqual()
    {
        var entry = EntryFactory.GetSingle();

        var detailedDto = entry.Adapt<EntryDetailedDto>();

        detailedDto.Id.ShouldBeEquivalentTo(entry.Id);
        detailedDto.CreatedDate.ShouldBeEquivalentTo(entry.CreatedDate);
        detailedDto.UpdatedDate.ShouldBeEquivalentTo(entry.UpdatedDate);

        detailedDto.EntryDate.ShouldBeEquivalentTo(entry.EntryDate);
        detailedDto.Note.ShouldBeEquivalentTo(entry.Note);
        detailedDto.ClientId.ShouldBeEquivalentTo(entry.ClientId);
        detailedDto.PassId.ShouldBeEquivalentTo(entry.PassId);
    }

    [Test]
    public void Adapt_DtoToEntry_ObjectTypeShouldBeEqualToAdapted()
    {
        var dto = EntryFactory.GetDto();

        var entry = dto.Adapt<Entry>();

        entry.GetType().ShouldBeEquivalentTo(typeof(Entry));
    }

    [Test]
    public void Adapt_DtoToEntry_AllValuesShouldBeEqual()
    {
        var dto = EntryFactory.GetDto();

        var entry = dto.Adapt<Entry>();

        entry.EntryDate.ShouldBeEquivalentTo(dto.EntryDate);
        entry.Note.ShouldBeEquivalentTo(dto.Note);
        entry.ClientId.ShouldBeEquivalentTo(dto.ClientId);
        entry.PassId.ShouldBeEquivalentTo(dto.PassId);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.Entries;

namespace Tuudio.Infrastructure.Data.Configurations;

public class EntryConfiguration : IEntityTypeConfiguration<Entry>
{
    public void Configure(EntityTypeBuilder<Entry> builder)
    {
        builder.HasKey(entry => entry.Id);

        builder.Property(entry => entry.EntryDate)
            .IsRequired();

        builder.HasData(
            new Entry()
            {
                Id = new Guid("00000000-0000-0000-0004-000000000001"),
                EntryDate = DateTime.Now,
                Note = "Client first entry without pass",
                ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                ActivityId = new Guid("00000000-0000-0000-0001-000000000001")
            },
            new Entry()
            {
                Id = new Guid("00000000-0000-0000-0004-000000000002"),
                EntryDate = DateTime.Now,
                Note = "Client second entry with pass",
                ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                PassId = new Guid("00000000-0000-0000-0003-000000000001"),
                ActivityId = new Guid("00000000-0000-0000-0001-000000000001")
            });

        builder.ToTable("Entries");
    }
}

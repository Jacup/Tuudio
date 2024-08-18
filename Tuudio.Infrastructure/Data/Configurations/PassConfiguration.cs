using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.Passes;

namespace Tuudio.Infrastructure.Data.Configurations;

public class PassConfiguration : IEntityTypeConfiguration<Pass>
{
    public void Configure(EntityTypeBuilder<Pass> builder)
    {
        builder.HasKey(pass => pass.Id);

        builder.Property(pass => pass.FromDate)
            .IsRequired();

        builder.Property(pass => pass.ToDate)
            .IsRequired();

        builder
            .HasMany(pass => pass.Entries)
            .WithOne(entry => entry.Pass)
            .HasForeignKey(pass => pass.PassId);

        builder.HasData(
            new Pass()
            {
                Id = new Guid("00000000-0000-0000-0003-000000000001"),
                FromDate = DateOnly.FromDateTime(DateTime.Now),
                ToDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(3)),
                ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
            });

        builder.Navigation(pass => pass.Entries).AutoInclude();

        builder.ToTable("Passes");
    }
}

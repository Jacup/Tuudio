using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.Passes;

namespace Tuudio.Infrastructure.Data.Configurations;

public class PassConfiguration : IEntityTypeConfiguration<Pass>
{
    public void Configure(EntityTypeBuilder<Pass> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FromDate)
            .IsRequired();

        builder.Property(e => e.ToDate)
            .IsRequired();

        builder.HasData(
            new Pass()
            {
                Id = new Guid("00000000-0000-0000-0003-000000000001"),
                FromDate = DateOnly.FromDateTime(DateTime.Now),
                ToDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(3)),
                ClientId = new Guid("00000000-0000-0000-0000-000000000001"),
                PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
            });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.Activities;
using Tuudio.Domain.Entities.PassTemplates;
using Tuudio.Domain.Enums;

namespace Tuudio.Infrastructure.Data.Configurations;

public class PassTemplateConfiguration : IEntityTypeConfiguration<PassTemplate>
{
    public void Configure(EntityTypeBuilder<PassTemplate> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasData(
            new PassTemplate
            {
                Id = new Guid("00000000-0000-0000-0010-000000000000"),
                Name = "Monthly yoga pass",
                Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
                Entries = 0
            });

        builder.OwnsOne(e => e.Price, price =>
        {
            price.Property(p => p.Amount).IsRequired();
            price.Property(p => p.Period).IsRequired();
        });

        builder.OwnsOne(e => e.Duration, duration =>
        {
            duration.Property(d => d.Amount).IsRequired();
            duration.Property(d => d.Period).IsRequired();
        });
    }
}
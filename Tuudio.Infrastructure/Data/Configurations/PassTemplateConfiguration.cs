using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.Passes;
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
                Id = new Guid("00000000-0000-0000-0002-000000000001"),
                Name = "Monthly yoga pass",
                Description = "Pass for yoga classes, unlimited entries, paid monthly, 3 months",
                EntriesAmount = 0
            });

        builder.OwnsOne(e => e.Price, price =>
        {
            price.Property(p => p.Amount).IsRequired();
            price.Property(p => p.Period).IsRequired();

            price.HasData(new
            {
                PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
                Amount = 100.0M,
                Period = Period.Month
            });
        });

        builder.OwnsOne(e => e.Duration, duration =>
        {
            duration.Property(d => d.Amount).IsRequired();
            duration.Property(d => d.Period).IsRequired();

            duration.HasData(new
            {
                PassTemplateId = new Guid("00000000-0000-0000-0002-000000000001"),
                Amount = 3,
                Period = Period.Month
            });
        });

        builder
            .HasMany(e => e.Activities)
            .WithMany(e => e.PassTemplates).UsingEntity(e =>
        e.HasData(
        [
            new { PassTemplatesId = new Guid("00000000-0000-0000-0002-000000000001"), ActivitiesId = new Guid("00000000-0000-0000-0001-000000000001") }
        ]));

        builder
            .HasMany(e => e.Passes)
            .WithOne(child => child.PassTemplate)
            .HasForeignKey(child => child.PassTemplateId)
            .IsRequired();

        builder.Navigation(e => e.Passes).AutoInclude();
        builder.Navigation(e => e.Activities).AutoInclude();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.Activities;

namespace Tuudio.Infrastructure.Data.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(e => e.Description)
            .HasMaxLength(128);

        builder.HasData(
            new Activity()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000001"),
                Name = "Yoga",
                Description = "Yoga group class"
            },
            new Activity()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000002"),
                Name = "Pool",
                Description = "Individual pool session"
            },
            new Activity()
            {
                Id = new Guid("00000000-0000-0000-0001-000000000003"),
                Name = "EMS training",
            });

        builder.ToTable("Activities");
    }
}

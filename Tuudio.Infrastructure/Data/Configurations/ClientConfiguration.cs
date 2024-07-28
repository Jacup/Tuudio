using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.People;

namespace Tuudio.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .HasMany(e => e.Passes)
            .WithOne(e => e.Client)
            .HasForeignKey(e => e.ClientId)
            .IsRequired();


        builder.HasData(
            new Client()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                FirstName = "John",
                LastName = "Doe",
                Email = "example@site.com",
            },
            new Client()
            {
                Id = new Guid("00000000-0000-0000-0000-000000000002"),
                FirstName = "Jane",
                LastName = "Doe",
            });

        builder.Navigation(e => e.Passes).AutoInclude();
        builder.ToTable("Clients");
    }
}

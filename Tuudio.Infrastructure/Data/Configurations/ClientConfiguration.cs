using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tuudio.Domain.Entities.People;

namespace Tuudio.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(client => client.Id);

        builder.Property(client => client.FirstName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(client => client.LastName)
            .IsRequired()
            .HasMaxLength(64);

        builder
            .HasMany(client => client.Passes)
            .WithOne(pass => pass.Client)
            .HasForeignKey(client => client.ClientId)
            .IsRequired();

        builder
            .HasMany(client => client.Entries)
            .WithOne(entry => entry.Client)
            .HasForeignKey(client => client.ClientId)
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

        builder.Navigation(client => client.Passes).AutoInclude();
        builder.Navigation(client => client.Entries).AutoInclude();
        
        builder.ToTable("Client");
    }
}

using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tuudio.Domain.Entities.People;

namespace Tuudio.Infrastructure.Data;

public class TuudioDbContext : DbContext
{
    public TuudioDbContext() : base()
    {
    }

    public TuudioDbContext(DbContextOptions<TuudioDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

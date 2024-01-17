using API.Domain;

namespace API.Database;

public sealed class Context : DbContext
{
    public Context(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) => builder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly).Seed();

    public DbSet<Domain.CronJob> CronJob { get; set; }

}

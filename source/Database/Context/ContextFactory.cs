namespace API.Database;

public sealed class ContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        const string connectionString = "Data Source=localhost;Initial Catalog=Glartek;Integrated Security=SSPI;TrustServerCertificate=True";

        return new Context(new DbContextOptionsBuilder<Context>().UseSqlServer(connectionString).Options);
    }
}

using System.Net.NetworkInformation;

namespace API.Database;

public static class ContextSeed
{
    public static void Seed(this ModelBuilder builder) => builder.Seeds();

    private static void Seeds(this ModelBuilder builder)
    {
    }
}

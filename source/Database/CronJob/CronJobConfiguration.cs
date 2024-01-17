using API.Domain;

namespace API.Database;

public sealed class CronJobConfiguration : IEntityTypeConfiguration<Domain.CronJob>
{
    public void Configure(EntityTypeBuilder<Domain.CronJob> builder)
    {
        builder.ToTable("CronJobs");

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id).ValueGeneratedOnAdd().IsRequired();

        builder.Property(entity => entity.Body).IsRequired();

        builder.Property(entity => entity.Schecule).IsRequired();

        builder.Property(entity => entity.HttpMethod).IsRequired();

        builder.Property(entity => entity.TimeZone).IsRequired();

    }
}

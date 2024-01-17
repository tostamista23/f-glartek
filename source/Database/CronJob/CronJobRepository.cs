using API.Domain;
using API.Model.Filters;

namespace API.Database;

public interface ICronJobRepository : IRepository<CronJob>
{
    Task<(long, IEnumerable<CronJob>)> ListAsync(BasicFilter filters);
}

public class CronJobRepository : EFRepository<CronJob>, ICronJobRepository
{
    private Context Context { get; }

    public CronJobRepository(Context context) : base(context) { Context = context; }


    public async Task<(long, IEnumerable<CronJob>)> ListAsync(BasicFilter filters)
    {
        var query = Context.CronJob.AsQueryable();

        return (query.Count(), await query.Skip((filters.PageIndex - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync());
    }
}

using API.Application;
using API.Model.Filters;

namespace API.Web;

[ApiController]
[Route("api/cronjobs")]
public sealed class CronJobController : ControllerBase
{
    private ICronJobService CronJobService { get; }

    public CronJobController(ICronJobService srv)
    {
        CronJobService = srv;
    }

    [HttpPost]
    public IActionResult Add(CronJobAdd dto) => CronJobService.CreateAsync(dto).ApiResult();

    [HttpGet("list")]
    public IActionResult List([FromQuery] BasicFilter dto) => CronJobService.ListAsync(dto).ApiResult();

    [HttpPut]
    public IActionResult Update(CronJobUpdate dto) => CronJobService.UpdateAsync(dto).ApiResult();

    [HttpGet("{id}")]
    public IActionResult Get(long id) => CronJobService.GetAsync(id).ApiResult();

    [HttpDelete("{id}")]
    public IActionResult Delete(long id) => CronJobService.DeleteAsync(id).ApiResult();

}

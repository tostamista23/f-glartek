using API.CrossCutting;

namespace API.Web;

[ApiController]
[Route("api/TimeZones")]
public sealed class TimeZoneController : ControllerBase
{
    public TimeZoneController() { }


    [HttpGet("list")]
    public IActionResult List() => TimeZoneService.GetTimeZones().ApiResult();


}

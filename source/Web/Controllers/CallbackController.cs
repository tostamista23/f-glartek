using API.Application;
using API.Model.Filters;

namespace API.Web;

[ApiController]
[Route("api/callback")]
public sealed class CallBackController : ControllerBase
{
    public CallBackController() { }

    [HttpPost]
    public IActionResult Post(CronJobAdd dto) => Ok("Criado com sucesso");

    [HttpPut]
    public IActionResult Put(CronJobUpdate dto) => Ok("Atualizado com sucesso");

    [HttpGet("{id}")]
    public IActionResult Get(long id) => Ok("Get com sucesso.");

    [HttpDelete("{id}")]
    public IActionResult Delete(long id) => Ok("Delete com sucesso.");

}

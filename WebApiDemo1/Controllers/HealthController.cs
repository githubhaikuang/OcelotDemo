using Microsoft.AspNetCore.Mvc;

namespace WebApiDemo1.Controllers
{
    [Route("api/Health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
}
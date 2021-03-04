using Microsoft.AspNetCore.Mvc;

namespace AireLogicTest.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArtistController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
using Mats.Edvardsen.TestingStuff.Web.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Mats.Edvardsen.TestingStuff.Web.SystemFeature;

[ApiController]
[Route("[controller]")]
public class SystemController(ApplicationSettings appSettings) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return new OkObjectResult(appSettings);
    }
}
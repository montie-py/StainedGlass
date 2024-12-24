using Microsoft.AspNetCore.Mvc;

namespace StainedGlass.Web.Controllers;

[Route("[controller]")]
public class AdminController : Controller
{
    [HttpGet("church")]
    public IActionResult Church()
    {
        return View();
    }
}
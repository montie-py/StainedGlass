using Microsoft.AspNetCore.Mvc;

namespace StainedGlass.Web.Controllers.Admin;

public abstract class AdminController : Controller
{
    public abstract IActionResult New();
    public abstract Task<IActionResult> All();
    public abstract Task<IActionResult> One(string slug);
    public abstract Task<IActionResult> Edit(string slug);
    public abstract Task<IActionResult> Delete(string slug);
}
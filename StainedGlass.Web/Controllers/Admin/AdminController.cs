using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StainedGlass.Web.Controllers.Admin;

[Authorize(Policy = "AdminOnly")]
public abstract class AdminController : Controller, ImageDisplayingInterface
{
    public abstract Task<IActionResult> New();
    public abstract Task<IActionResult> All();
    public abstract Task<IActionResult> One(string slug);
    public abstract Task<IActionResult> Edit(string slug);
    public abstract Task<IActionResult> Delete(string slug);
    
}
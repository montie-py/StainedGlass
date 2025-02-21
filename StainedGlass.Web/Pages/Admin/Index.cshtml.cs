using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StainedGlass.Web.Pages.Admin;

[Authorize(Policy = "AdminOnly")]
public class Index : PageModel
{
    public void OnGet()
    {
        
    }
}
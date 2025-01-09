using Microsoft.AspNetCore.Mvc;
using StainedGlass.Persistence.Templates;
using StainedGlass.Transfer;
using StainedGlass.Transfer.DTOs;

namespace StainedGlass.Web.Controllers.Admin;

[Route("admin/[controller]")]
public class SanctuarySideController : AdminController
{
    private InputBoundary _useCaseInteractor;
    public SanctuarySideController(InputBoundary useCaseInteractor)
    {
        _useCaseInteractor = useCaseInteractor;
        ((UseCaseInteractor)_useCaseInteractor).SetPersistenceTemplate(new DatabasePersistenceTemplate());
    }
    
    [HttpGet("sanctuaryside")]
    public override async Task<IActionResult> New()
    {
        ViewBag.Churches = await _useCaseInteractor.GetAllDTOs<ChurchDTO>();
        if (ViewBag.Churches != null)
        {
            ViewBag.ChurchesImages = new Dictionary<string, string>();
            foreach (var church in ViewBag.Churches)
            {
                ViewBag.ChurchesImages[church.Slug] = await IFormFileToBase64(church.Image);
            }
        }
        return View("Admin/SanctuarySide/NewSanctuarySide");
    }

    [HttpGet("sanctuarysides")]
    public override async Task<IActionResult> All()
    {
        ViewBag.SanctuarySides = await _useCaseInteractor.GetAllDTOs<SanctuarySideDTO>();
        return View("Admin/SanctuarySide/SanctuarySides");
    }

    [HttpGet("{slug}")]
    public override async Task<IActionResult> One(string slug)
    {
        ViewBag.SanctuarySide = await _useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(slug);
        if (ViewBag.SanctuarySide.Church.Image != null && ViewBag.SanctuarySide.Church.Image.Length > 0)
        {
            ViewBag.ChurchImage = await IFormFileToBase64(ViewBag.SanctuarySide.Church.Image);
        }
        return View("Admin/SanctuarySide/SanctuarySide");
    }

    [HttpGet("{slug}/edit")]
    public override async Task<IActionResult> Edit(string slug)
    {
        ViewBag.SanctuarySide = await _useCaseInteractor.GetDTOBySlug<SanctuarySideDTO>(slug);
        ViewBag.Churches = await _useCaseInteractor.GetAllDTOs<ChurchDTO>();
        if (ViewBag.Churches != null)
        {
            ViewBag.ChurchesImages = new Dictionary<string, string>();
            foreach (var church in ViewBag.Churches)
            {
                ViewBag.ChurchesImages[church.Slug] = await IFormFileToBase64(church.Image);
            }
        }

        return View("Admin/SanctuarySide/EditSanctuarySide"); 
    }

    [HttpDelete("{slug}")]
    public override async Task<IActionResult> Delete(string slug)
    {
        await _useCaseInteractor.RemoveEntity<SanctuarySideDTO>(slug);
        return Ok();
    }
}
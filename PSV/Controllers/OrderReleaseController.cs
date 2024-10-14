using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

public class OrderReleaseController : Controller
{
    private readonly IDbService _service;

    public OrderReleaseController(IDbService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> NewReleaser()
    {
        ReleaserPost newReleaser = new ReleaserPost();
        var allLocations = await _service.GetAllLocations();
        newReleaser.AllLocations = allLocations;
        return View("CreateReleaser", newReleaser);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReleaser(ReleaserPost newReleaser)
    {
        if (ModelState.IsValid)
        {
            await _service.AddReleaser(newReleaser);
            return RedirectToAction("AllReleasers");
        }
        var allLocations = await _service.GetAllLocations();
        newReleaser.AllLocations = allLocations;
        return View("CreateReleaser", newReleaser);
    }

    [HttpGet]
    public async Task<IActionResult> AllReleasers(int pageNumber = 1, int pageSize = 20)
    {
        var releasers = await _service.GetReleasers();
        var totalItems = releasers.Count();
        var releasersOnPage = releasers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        var paginatedReleasers = new PaginationViewModel<ReleaserDetails>(releasersOnPage, totalItems, pageNumber, pageSize);
        return View("ListReleaser", paginatedReleasers);
    }

}
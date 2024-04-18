using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class ClientController : Controller
{
    private readonly IDbService _service;

    [HttpGet]
    public IActionResult New()
    {
        return View("Create");
    }

    
    [HttpPost]
    public async Task<IActionResult> Create(ClientPost request)
    {
        await _service.AddClient(request);
        return RedirectToAction("All");
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var dto = await _service.GetAllClients();
        return View("List", dto);
    }
}
using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class ClientController : Controller
{
    private readonly IDbService _service;

    public ClientController(IDbService service)
    {
        _service = service;
    }
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
        var clients = await _service.GetAllClients();
        return View("List", clients);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _service.GetClientDetails(id);
        return View("Details", dto);
    }

    [HttpGet]
    public async Task<IActionResult> EditForm(int id)
    {
        var dto = await _service.GetClientDetails(id);
        return View("Edit", dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ClientPost client)
    {
        try
        {
            await _service.EditClient(id, client);
            return RedirectToAction("All");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteClient(id);
            return RedirectToAction("All");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
}
using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class MillingController : Controller
{
    private readonly IDbService _service;

    public MillingController(IDbService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Menu()
    {
        return View("Menu");
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var dto = await _service.GetAllOrders();
        return View("List", dto);
    }

    [HttpGet]
    public async Task<IActionResult> Search()
    {
        return View("Search");
    }

    public async Task<IActionResult> Order(int id)
    {
        var dto = await _service.GetMillingControlData(id);
        return View("Control", dto);
    }
    
    [HttpGet]
    public async Task<IActionResult> UpdateMillingStartTime(int id)
    {
        await _service.UpdateMillingStartTime(id);
        return RedirectToAction("Order", new {id});
    }

    [HttpGet]
    public async Task<IActionResult> UpdateMillingEndTime(int id)
    {
        await _service.UpdateMillingEndTime(id);
        return RedirectToAction("Order", new {id});
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(OrderControl dto)
    {
        await _service.CommentOrder(dto);
        return RedirectToAction("Order", new { dto.Id });
    }
}
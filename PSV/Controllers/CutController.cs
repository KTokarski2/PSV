using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class CutController : Controller
{
    private readonly IDbService _service;

    public CutController(IDbService service)
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
    public IActionResult Search()
    {
        return View("Search");
    }

    [HttpGet]
    public async Task<IActionResult> Find(string orderNumber)
    {
        var id = await _service.GetIdByOrderNumber(orderNumber);
        if (id == null)
        {
            return View("NotFound");
        }

        var dto = await _service.GetCutControlData(id);
        return View("Control", dto);
    }

    public async Task<IActionResult> Order(int id, bool startTimer)
    {
        var dto = await _service.GetCutControlData(id);
        dto.StartTimer = startTimer;
        return View("Control", dto);
    }
    
    public async Task<IActionResult> UpdateCutStartTime(int id)
    {
        await _service.UpdateCutStartTime(id);
        const bool startTimer = true;
        return RedirectToAction("Order", new { id, startTimer });
    }
    
    public async Task<IActionResult> UpdateCutEndTime(int id)
    {
        await _service.UpdateCutEndTime(id);
        return RedirectToAction("Order", new { id });
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(OrderControl dto)
    {
        await _service.CommentOrder(dto);
        return RedirectToAction("Order", new { dto.Id });
    }
    
}
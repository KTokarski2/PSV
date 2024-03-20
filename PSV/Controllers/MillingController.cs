using Microsoft.AspNetCore.Mvc;
using PSV.Services;

namespace PSV.Controllers;

public class MillingController : Controller
{
    private readonly IDbService _service;

    public MillingController(IDbService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Order()
    {
        return View("Control");
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateMillingStartTime(int orderId)
    {
        await _service.UpdateMillingStartTime(orderId);
        return RedirectToAction();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateMillingEndTime(int orderId)
    {
        await _service.UpdateMillingEndTime(orderId);
        return RedirectToAction();
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateWrappingStartTime(int orderId)
    {
        await _service.UpdateWrappingStartTime(orderId);
        return RedirectToAction();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateWrappingEndTime(int orderId)
    {
        await _service.UpdateWrappingEndTime(orderId);
        return RedirectToAction();
    }

}
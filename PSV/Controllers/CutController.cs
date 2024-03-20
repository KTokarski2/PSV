using Microsoft.AspNetCore.Mvc;
using PSV.Services;

namespace PSV.Controllers;

public class CutController : Controller
{
    private readonly IDbService _service;

    public CutController(IDbService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Order()
    {
        return View("Control");
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateCutStartTime(int orderId)
    {
        await _service.UpdateCutStartTime(orderId);
        return RedirectToAction();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCutEndTime(int orderId)
    {
        await _service.UpdateCutEndTime(orderId);
        return RedirectToAction();
    }

}
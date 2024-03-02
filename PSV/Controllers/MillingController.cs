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
}
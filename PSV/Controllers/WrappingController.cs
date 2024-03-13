using Microsoft.AspNetCore.Mvc;
using PSV.Services;

namespace PSV.Controllers;

public class WrappingController : Controller
{
    private readonly IDbService _service;

    public WrappingController(IDbService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Order()
    {
        return View("Control");
    }
}
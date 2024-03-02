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
}
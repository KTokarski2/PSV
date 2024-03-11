using Microsoft.AspNetCore.Mvc;
using PSV.Models;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class OrderController : Controller
{

    private readonly IDbService _service;

    public OrderController(IDbService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        return View("List");
    }

    [HttpGet]
    public async Task<IActionResult> New()
    {
        return View("Create");
    }

    // [HttpPost]
    // public async Task<IActionResult> Create(Domain.Entities.DTOs.OrderPost request)
    // {
    //     await _service.AddOrder(request);
    //     return RedirectToAction("List");
    // }

}
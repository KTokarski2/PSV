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

    [HttpGet]
    public async Task<IActionResult> Details()
    {
        return View("Details");
    }

    [HttpGet]
    public async Task<IActionResult> Search()
    {
        return View("Search");
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderPost request)
    {
        await _service.AddOrder(request);
        return RedirectToAction("New");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        await _service.GetAllOrders();
        return RedirectToAction("List");
    }

}
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
    public async Task<IActionResult> All()
    {
        var dto = await _service.GetAllOrders();
        return View("List", dto);
    }

    [HttpGet]
    public async Task<IActionResult> New()
    {
        return View("Create");
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(OrderPost request)
    {
        await _service.AddOrder(request);
        return RedirectToAction("All");
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _service.GetOrderDetails(id);
        return View("Details", dto);
    }

    [HttpGet]
    public async Task<IActionResult> Search()
    {
        return View("Search");
    }

    public async Task<IActionResult> GetQrCode(int id)
    {
        /*
        var path = await _service.GetQrCodePath(id);
        if (System.IO.File.Exists(path))
        {
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(path);
            Response.ContentType = "image/png";
            Response.Headers("")
            Response.AppendHeader("Content-Disposition", "attachment; filename=kodQR.png");
        }
        */
        return RedirectToAction("Details", new { id });
    }
    
}
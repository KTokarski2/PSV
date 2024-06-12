using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        OrderPost newOrder = new OrderPost();
        var allClients = await _service.GetClientsInfo();
        newOrder.AllClients = allClients;
        return View("Create", newOrder);
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
    public async Task<IActionResult> EditForm(int id)
    {
        var dto = await _service.GetOrderDetails(id);
        return View("Edit", dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(OrderDetails dto)
    {
        await _service.EditOrder(dto);
        return RedirectToAction("Details", new { dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Search()
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
        
        var order = await _service.GetOrderDetails(id);
        return View("Details", order);
    }

    public async Task<IActionResult> GetQrCode(int id)
    {
        var path = await _service.GetQrCodePath(id);
        var fileName = Path.GetFileName(path);
        var fileContent = await System.IO.File.ReadAllBytesAsync(path);

        return File(fileContent, "application/octet-stream", fileName);
    }

    public async Task<IActionResult> GetBarcode(int id)
    {
        var path = await _service.GetBarcodePath(id);
        var fileName = Path.GetFileName(path);
        var fileContent = await System.IO.File.ReadAllBytesAsync(path);
        return File(fileContent, "application/octet-stream", fileName);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteOrder(id);
        return RedirectToAction("All");
    }

    [HttpGet]
    public async Task<IActionResult> GetCodeByID(int id)
    {
        await _service.GetCodeByID(id);
        return RedirectToAction("All");
    }
}
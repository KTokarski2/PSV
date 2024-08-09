using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using PSV.Models;
using PSV.Models.DTOs;
using PSV.Services;
using PSV.Utils;

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

    [HttpPost]
    public async Task<IActionResult> UploadOrderFile(OrderPost request)
    {
        OrderPost newOrder = new OrderPost();
        OrderDataService dataService = new OrderDataService();
        var allClients = await _service.GetClientsInfo();
        var allLocations = await _service.GetAllLocations();
        newOrder.AllLocations = allLocations;
        newOrder.AllClients = allClients;
        newOrder.OrderNumber = dataService.ExtractOrderNumber(request.OrderFile);
        await dataService.SaveTemporaryFile(request.OrderFile);
        ModelState.Remove("OrderNumber");
        return View("Create", newOrder);
    }

    [HttpGet]
    public async Task<IActionResult> New()
    {
        OrderPost newOrder = new OrderPost();
        var allClients = await _service.GetClientsInfo();
        var allLocations = await _service.GetAllLocations();
        newOrder.AllClients = allClients;
        newOrder.AllLocations = allLocations;
        return View("Create", newOrder);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(OrderPost request)
    {
        if (ModelState.IsValid)
        {
            await _service.AddOrder(request);
            return RedirectToAction("All");
        }

        var allClients = await _service.GetClientsInfo();
        var allLocations = await _service.GetAllLocations();
        request.AllClients = allClients;
        request.AllLocations = allLocations;
        return View("Create", request);
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
        dto.AllLocations = await _service.GetAllLocations();
        return View("Edit", dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(OrderDetails dto)
    {
        if (ModelState.IsValid)
        {
            await _service.EditOrder(dto);
            return RedirectToAction("Details", new { dto.Id });
        }

        dto.AllLocations = await _service.GetAllLocations();
        dto.AllClients = await _service.GetClientsInfo();
        return View("Edit", dto);
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
    
    public async Task<IActionResult> PrintBarcode(int id)
    {
        var path = await _service.GetBarcodePath(id);
        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }

        var fileName = Path.GetFileName(path);
        var mimeType = "image/png";
        var fileContent = await System.IO.File.ReadAllBytesAsync(path);
        var base64Image = Convert.ToBase64String(fileContent);
        var imageSrc = $"data:{mimeType};base64,{base64Image}";

        var viewModel = new BarCodeViewModel
        {
            Id = id,
            ImageSrc = imageSrc
        };

        return View("PrintBarcode", viewModel);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteOrder(id);
        return RedirectToAction("All");
    }

    public async Task<IActionResult> ViewComments(int id)
    {
        var dto = new CommentsDto();
        var order = await _service.GetOrderDetails(id);
        dto.Id = order.Id;
        dto.OrderNumber = order.OrderNumber;
        var comments = await _service.GetOrderComments(id);
        dto.Comments = comments;
        return View("Comments", dto);
    }
}
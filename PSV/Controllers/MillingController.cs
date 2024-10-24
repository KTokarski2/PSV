using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class MillingController : Controller
{
    private readonly IDbService _service;

    public MillingController(IDbService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Menu()
    {
        return View("Menu");
    }

    [HttpGet]
    public async Task<IActionResult> All(int pageNumber = 1, int pageSize = 20)
    {
        var orders = await _service.GetAllOrders();
        var totalItems = orders.Count;
        var ordersOnPage = orders
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var paginatedOrders = new PaginationViewModel<OrderList>(ordersOnPage, totalItems, pageNumber, pageSize);
        
        
        return View("List", paginatedOrders);
    }
    
    [HttpGet]
    public async Task<IActionResult> Find(string orderNumber)
    {
        var id = await _service.GetIdByOrderNumber(orderNumber);
        if (id == null)
        {
            return View("NotFound");
        }
        var dto = await _service.GetMillingControlData(id);
        if (await _service.IsMillingPresent(id))
        {
            dto.Operators = await _service.GetAllOperators();
            return View("Control", dto);
        }

        return View("NotPresent", dto);
    }

    public async Task<IActionResult> Order(int id, bool startTimer, int operatorId)
    {
        var dto = await _service.GetMillingControlData(id);
        if (await _service.IsMillingPresent(id))
        {
            dto.StartTimer = startTimer;
            dto.Operators = await _service.GetAllOperators();
            dto.OperatorId = operatorId;
            return View("Control", dto);
        }

        return View("NotPresent", dto);
    }
    
    [HttpGet]
    public async Task<IActionResult> UpdateMillingStartTime(int id, int operatorId)
    {
        await _service.UpdateMillingStartTime(id, operatorId);
        const bool startTimer = true;
        return RedirectToAction("Order", new {id, startTimer, operatorId});
    }

    [HttpGet]
    public async Task<IActionResult> UpdateMillingEndTime(int id, int operatorId)
    {
        await _service.UpdateMillingEndTime(id, operatorId);
        return RedirectToAction("Comment", new {id});
    }

    [HttpGet]
    public async Task<IActionResult> Comment(int id)
    {
        var dto = await _service.GetMillingControlData(id);
        return View("CommentsDecision", dto);
    }

    [HttpGet]
    public async Task<IActionResult> CommentsForm(int id)
    {
        var dto = await _service.GetMillingControlData(id);
        dto.OperatorId = await _service.GetMillingOperatorId(id);
        return View("Comments", dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(OrderControl dto)
    {
        await _service.CommentOrder(dto, "frezowanie");
        return RedirectToAction("Menu");
    }

    public async Task<IActionResult> RedirectToRelease()
    {
        var pg = new RedirectionModel { Controller = "Milling", Method = "Menu", ButtonText = "stanowiska frezowania" };
        TempData["PreviousPage"] = Newtonsoft.Json.JsonConvert.SerializeObject(pg);
        return RedirectToAction("ReleaseMenu", "OrderRelease");
    }
}
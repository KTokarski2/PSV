using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class CutController : Controller
{
    private readonly IDbService _service;

    public CutController(IDbService service)
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
        var dto = await _service.GetCutControlData(id);

        if (await _service.IsCutPresent(id))
        {
            dto.Operators = await _service.GetAllOperators();
            return View("Control", dto);
        }

        return View("NotPresent", dto);
    }

    public async Task<IActionResult> Order(int id, bool startTimer, int operatorId)
    {
        var dto = await _service.GetCutControlData(id);

        if (await _service.IsCutPresent(id))
        {
            dto.StartTimer = startTimer;
            dto.Operators = await _service.GetAllOperators();
            dto.OperatorId = operatorId;
            return View("Control", dto);
        }

        return View("NotPresent", dto);
    }
    
    public async Task<IActionResult> UpdateCutStartTime(int id, int operatorId)
    {
        await _service.UpdateCutStartTime(id, operatorId);
        const bool startTimer = true;
        return RedirectToAction("Order", new { id, startTimer, operatorId });
    }
    
    public async Task<IActionResult> UpdateCutEndTime(int id, int operatorId)
    {
        await _service.UpdateCutEndTime(id, operatorId);
        return RedirectToAction("Comment", new {id});
    }

    [HttpGet]
    public async Task<IActionResult> Comment(int id)
    {
        var dto = await _service.GetCutControlData(id);
        return View("CommentsDecision", dto);
    }

    [HttpGet]
    public async Task<IActionResult> CommentsForm(int id)
    {
        var dto = await _service.GetCutControlData(id);
        dto.OperatorId = await _service.GetCutOperatorId(id);
        return View("Comments", dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(OrderControl dto)
    {
        await _service.CommentOrder(dto, "cięcie");
        return RedirectToAction("Menu");
    }

    public async Task<IActionResult> RedirectToRelease()
    {
        var pg = new RedirectionModel { Controller = "Cut", Method = "Menu", ButtonText = "stanowiska cięcia"};
        TempData["PreviousPage"] = Newtonsoft.Json.JsonConvert.SerializeObject(pg);
        return RedirectToAction("ReleaseMenu", "OrderRelease");
    }
    
}
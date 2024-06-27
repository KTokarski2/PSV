using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class WrappingController : Controller
{
    private readonly IDbService _service;

    public WrappingController(IDbService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Menu()
    {
        return View("Menu");
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var dto = await _service.GetAllOrders();
        return View("List", dto);
    }

    [HttpGet]
    public async Task<IActionResult> Find(string orderNumber)
    {
        var id = await _service.GetIdByOrderNumber(orderNumber);
        if (id == null)
        {
            return View("NotFound");
        }
        var dto = await _service.GetWrappingControlData(id);

        if (await _service.IsWrappingPresent(id))
        {
            dto.Operators = await _service.GetAllOperators();
            return View("Control", dto);
        }

        return View("NotPresent", dto);
    }
    
    public async Task<IActionResult> Order(int id, bool startTimer)
    {
        var dto = await _service.GetWrappingControlData(id);
        if (await _service.IsWrappingPresent(id))
        {
            dto.StartTimer = startTimer;
            dto.Operators = await _service.GetAllOperators();
            return View("Control", dto);
        }

        return View("NotPresent", dto);
    }
    
    [HttpGet]
    public async Task<IActionResult> UpdateWrappingStartTime(int id, int operatorId, string edgeCode)
    {
        await _service.UpdateWrappingStartTime(id, operatorId);
        const bool startTimer = true;
        if (await _service.CheckIfUsedDifferentProvided(id, edgeCode))
        {
            await _service.SetUsedEdgeCode(id, edgeCode);
        }
        return RedirectToAction("Order", new {id, startTimer});
    }

    [HttpGet]
    public async Task<IActionResult> UpdateWrappingEndTime(int id, int operatorId, string edgeCode)
    {
        await _service.UpdateWrappingEndTime(id, operatorId);
        if (await _service.CheckIfUsedDifferentProvided(id, edgeCode))
        {
            await _service.SetUsedEdgeCode(id, edgeCode);
        }
        return RedirectToAction("Comment", new {id});
    }

    [HttpGet]
    public async Task<IActionResult> Comment(int id)
    {
        var dto = await _service.GetWrappingControlData(id);
        return View("CommentsDecision", dto);
    }

    [HttpGet]
    public async Task<IActionResult> CommentsForm(int id)
    {
        var dto = await _service.GetWrappingControlData(id);
        return View("Comments", dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(OrderControl dto)
    {
        await _service.CommentOrder(dto);
        return RedirectToAction("Menu");
    }
}
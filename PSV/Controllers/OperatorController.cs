using Microsoft.AspNetCore.Mvc;
using PSV.Models;
using PSV.Models.DTOs;
using PSV.Services;

namespace PSV.Controllers;

public class OperatorController : Controller
{
    private readonly IDbService _service;

    public OperatorController(IDbService service)
    {
        _service = service;
    }

    

    [HttpGet]
    public async Task<IActionResult> New()
    {
        return View("Create");
    }

    [HttpPost]
    public async Task<IActionResult> Create(OperatorPost newOperator)
    {
        await _service.AddOperator(newOperator);
        return RedirectToAction("All");
    }
    
    [HttpGet]
    public async Task<IActionResult> All()
    {
        var dto = await _service.GetOperators();
        return View("List", dto);
    }
    
    
}
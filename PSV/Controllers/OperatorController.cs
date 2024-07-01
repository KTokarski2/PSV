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
        OperatorPost newOperator = new OperatorPost();
        var allLocations = await _service.GetAllLocations();
        newOperator.AllLocations = allLocations;
        return View("Create", newOperator);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OperatorPost newOperator)
    {
        if (ModelState.IsValid)
        {
            await _service.AddOperator(newOperator);
            return RedirectToAction("All");
        }

        var allLocations = await _service.GetAllLocations();
        newOperator.AllLocations = allLocations;
        return View("Create", newOperator);
    }
    
    [HttpGet]
    public async Task<IActionResult> All()
    {
        var dto = await _service.GetOperators();
        return View("List", dto);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var dto = await _service.GetOperatorDetails(id);
        return View("Details", dto);
    }

    [HttpGet]
    public async Task<IActionResult> EditForm(int id)
    {
        var dto = await _service.GetOperatorDetails(id);
        var allLocations = await _service.GetAllLocations();
        dto.AllLocations = allLocations;
        return View("Edit", dto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, OperatorDetails opr)
    {
        if (ModelState.IsValid)
        {
            await _service.EditOperator(id, opr);
            return RedirectToAction("Details", new {id});
        }

        opr.AllLocations = await _service.GetAllLocations();
        return View("Edit", opr);
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteOperator(id);
            return RedirectToAction("All");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    
}
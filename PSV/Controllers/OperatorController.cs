using Microsoft.AspNetCore.Mvc;
using PSV.Models;
using PSV.Services;

namespace PSV.Controllers;

public class OperatorController : Controller
{
    private readonly IDbService _service;

    public OperatorController(IDbService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddOperator([FromBody] Operator newOperator)
    {
        await _service.AddOperator(newOperator);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> EditOperator([FromBody] Operator updatedOperator)
    {
        try
        {
            await _service.EditOperator(updatedOperator);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetOperators()
    {
        var operators = await _service.GetOperators();
        return Ok(operators);
    }
}
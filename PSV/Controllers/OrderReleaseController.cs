using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;
using PSV.Services;

public class OrderReleaseController : Controller
{
    private readonly IDbService _service;

    public OrderReleaseController(IDbService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> NewReleaser()
    {
        ReleaserPost newReleaser = new ReleaserPost();
        var allLocations = await _service.GetAllLocations();
        newReleaser.AllLocations = allLocations;
        return View("CreateReleaser", newReleaser);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReleaser(ReleaserPost newReleaser)
    {
        if (ModelState.IsValid)
        {
            await _service.AddReleaser(newReleaser);
            return RedirectToAction("AllReleasers");
        }
        var allLocations = await _service.GetAllLocations();
        newReleaser.AllLocations = allLocations;
        return View("CreateReleaser", newReleaser);
    }

    [HttpGet]
    public async Task<IActionResult> AllReleasers(int pageNumber = 1, int pageSize = 20)
    {
        var releasers = await _service.GetReleasers();
        var totalItems = releasers.Count();
        var releasersOnPage = releasers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        var paginatedReleasers = new PaginationViewModel<ReleaserDetails>(releasersOnPage, totalItems, pageNumber, pageSize);
        return View("ListReleaser", paginatedReleasers);
    }

    [HttpGet]
    public async Task<IActionResult> ReleaserDetails(int id)
    {
        var dto = await _service.GetReleaserDetails(id);
        return View("DetailsReleaser", dto);
    }

    [HttpGet]
    public async Task<IActionResult> ReleaserEditForm(int id)
    {
        var dto = await _service.GetReleaserDetails(id);
        dto.AllLocations = await _service.GetAllLocations();
        return View("EditReleaser", dto);
    }

    [HttpPost]
    public async Task<IActionResult> ReleaserEdit(int id, ReleaserDetails dto)
    {
        if (ModelState.IsValid)
        {
            await _service.EditReleaser(id, dto);
            return RedirectToAction("ReleaserDetails", new { id });
        }
        dto.AllLocations = await _service.GetAllLocations();
        return View("EditReleaser", dto);
    }

    public async Task<IActionResult> DeleteReleaser(int id)
    {
        await _service.DeleteReleaser(id);
        return RedirectToAction("AllReleasers");
    }

    [HttpGet]
    public async Task<IActionResult> ReleaseMenu()
    {
        var page = Newtonsoft.Json.JsonConvert.DeserializeObject<RedirectionModel>(TempData["PreviousPage"].ToString());
        TempData["PreviousPage"] = Newtonsoft.Json.JsonConvert.SerializeObject(page);
        return View("OrderReleaseMenu", page);
    }

    [HttpGet]
    public async Task<IActionResult> Find(string orderNumber)
    {
        var orderId = await _service.GetIdByOrderNumber(orderNumber);
        var page = Newtonsoft.Json.JsonConvert.DeserializeObject<RedirectionModel>(TempData["PreviousPage"].ToString());
        TempData["PreviousPage"] = Newtonsoft.Json.JsonConvert.SerializeObject(page);
        var se = new SearchError();
        se.Redirection = page;
        if (orderId == null)
        {
            se.Message = "Zamówienie o podanym numerze nie istnieje";
            return View("SearchError", se);
        }

        if (await _service.CheckIfAlreadyReleased(orderId))
        {
            se.Message = "Zamówienie zostało już wydane";
            return View("SearchError", se);
        }
        if (await _service.CheckIfReadyForRelease(orderId))
        {
            var dto = await _service.GetReleaseOrderData(orderId);
            dto.Redirection = page;
            return View("ReleaseOrder", dto);
        }
        se.Message = "Zamówienie nie jest gotowe do wydania";
        return View("SearchError", se);
    }

    public async Task<IActionResult> ReleaseOrder(int id, int releaserId, string originController, string originAction)
    {
        await _service.ReleaseOrder(id, releaserId);
        return RedirectToAction(originAction, originController);
    }
}
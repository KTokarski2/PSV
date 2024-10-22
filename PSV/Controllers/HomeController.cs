using Microsoft.AspNetCore.Mvc;
using PSV.Models.DTOs;


namespace PSV.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult RedirectToReleaseMenu()
    {
        var previousPage = new RedirectionModel { Method = "Index", Controller = "Home", ButtonText = "menu głównego"};
        TempData["PreviousPage"] = Newtonsoft.Json.JsonConvert.SerializeObject(previousPage);
        return RedirectToAction("ReleaseMenu", "OrderRelease");
    }
    
    
}
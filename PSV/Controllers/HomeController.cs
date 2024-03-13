using Microsoft.AspNetCore.Mvc;


namespace PSV.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    
}
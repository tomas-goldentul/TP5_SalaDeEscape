using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using salaEscape.Models;

namespace salaEscape.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult sala6()
    {
        const int JUGADAS_MAXIMAS = 5;
        ViewBag.perdio = ObjetoUtils.StringToObject<bool>(HttpContext.Session.GetString("perdio"));
        ViewBag.cantJugadas = ObjetoUtils.StringToObject<int>(HttpContext.Session.GetString("jugadas"));
        ViewBag.coloresJugados = HttpContext.Session.SetString("letrasIngresadas", ObjetoLista.ListToString(Sala6.letrasIngresadas()));
        if (ViewBag.perdio == true)
        {
            return View("derrota");
        }
        else if (ViewBag.perdio == true && ViewBag.jugadas == JUGADAS_MAXIMAS)
        {
            return View("salaFinal");

        }
        else{
            return View();
        }

    }
}

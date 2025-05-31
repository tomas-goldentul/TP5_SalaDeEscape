using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using salaEscape.Models;

namespace salaEscape.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private const string SALA_KEY = "Sala";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Creditos()
    {
        return View();
    }

    public IActionResult Salas()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(SALA_KEY)))
        {
            HttpContext.Session.SetString(SALA_KEY, "1");
        }
        return View();
    }

    public IActionResult Historia()
    {
        return View();
    }

    public IActionResult Sala1(int clave)
    {
        var sala = new Sala1();
        if(sala.Verificar(clave))
        {
            HttpContext.Session.SetString(SALA_KEY, "2");
            return RedirectToAction("Sala2");
        }
        return View();
    }

    public IActionResult Sala2()
    {
        if (!ValidarProgresoSala(2)) return RedirectToAction("Salas");
        return View();
    }

    public IActionResult Sala3(int clave)
    {
        if (!ValidarProgresoSala(3)) return RedirectToAction("Salas");
        
        if(new Sala3().Verificar(clave))
        {
            HttpContext.Session.SetString(SALA_KEY, "4");
            return RedirectToAction("Sala4");
        }
        return View();
    }

    public IActionResult Sala4()
    {
        if (!ValidarProgresoSala(4)) return RedirectToAction("Salas");
        return View();
    }

    public IActionResult Sala5(int clave)
    {
        if (!ValidarProgresoSala(5)) return RedirectToAction("Salas");
        
        if(new Sala5().Verificar(clave))
        {
            HttpContext.Session.SetString(SALA_KEY, "6");
            return RedirectToAction("Sala6");
        }
        return View();
    }

    public IActionResult Sala6(char letra)
    {
        if (!ValidarProgresoSala(6)) return RedirectToAction("Salas");

        var sala6 = ObjetoUtils.StringToObject<Sala6>(HttpContext.Session.GetString("Sala6")) ?? new Sala6();
        
        if (sala6.Jugar(letra))
        {
            if (sala6.HaGanado())
            {
                return RedirectToAction("SalaFinal");
            }
            return RedirectToAction("Derrota");
        }

        HttpContext.Session.SetString("Sala6", ObjetoUtils.ObjectToString(sala6));
        
        ViewBag.LetrasRandom = sala6.LetrasRandom;
        ViewBag.LetrasIngresadas = sala6.LetrasIngresadas;
        ViewBag.Jugadas = sala6.Jugadas;
        ViewBag.Perdio = sala6.Perdio;
        
        return View();
    }

    public IActionResult SalaFinal()
    {
        return View();
    }

    public IActionResult Derrota()
    {
        return View();
    }

    private bool ValidarProgresoSala(int salaActual)
    {
        var salaMaxima = HttpContext.Session.GetString(SALA_KEY);
        if (string.IsNullOrEmpty(salaMaxima)) return false;
        return int.Parse(salaMaxima) >= salaActual;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

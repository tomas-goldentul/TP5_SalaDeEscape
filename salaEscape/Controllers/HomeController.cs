using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using salaEscape.Models;

namespace salaEscape.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private const string SALA_KEY = "Sala";
    private const string QTE_TERMINADO_KEY = "QteTerminado";
    private const string AHORCADO_KEY = "Ahorcado";

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

        var sala2 = ObjetoUtils.StringToObject<Sala2>(HttpContext.Session.GetString(AHORCADO_KEY));
        if (sala2 == null)
        {
            sala2 = new Sala2();
            sala2.crearRandom();
            HttpContext.Session.SetString(AHORCADO_KEY, ObjetoUtils.ObjectToString(sala2));
        }

        ViewBag.PalabraJugada = sala2.palabraJugada;
        ViewBag.LetrasErradas = sala2.letrasErradas;
        ViewBag.Intentos = 6 - sala2.letrasErradas.Count;
        
        return View();
    }

    [HttpPost]
    public IActionResult IngresarLetra(char letra)
    {
        if (!ValidarProgresoSala(2)) return RedirectToAction("Salas");

        var sala2 = ObjetoUtils.StringToObject<Sala2>(HttpContext.Session.GetString(AHORCADO_KEY));
        if (sala2 == null)
        {
            return RedirectToAction("Sala2");
        }

        if (sala2.ingresarLetra(letra))
        {
            HttpContext.Session.SetString(SALA_KEY, "3");
            return RedirectToAction("Sala3");
        }

        if (sala2.letrasErradas.Count >= 6)
        {
            return RedirectToAction("Derrota");
        }

        HttpContext.Session.SetString(AHORCADO_KEY, ObjetoUtils.ObjectToString(sala2));
        return RedirectToAction("Sala2");
    }

    [HttpPost]
    public IActionResult IngresarPalabra(string palabra)
    {
        if (!ValidarProgresoSala(2)) return RedirectToAction("Salas");

        var sala2 = ObjetoUtils.StringToObject<Sala2>(HttpContext.Session.GetString(AHORCADO_KEY));
        if (sala2 == null)
        {
            return RedirectToAction("Sala2");
        }

        if (sala2.ingresarPalabra(palabra))
        {
            HttpContext.Session.SetString(SALA_KEY, "3");
            return RedirectToAction("Sala3");
        }
        return RedirectToAction("Derrota");
    }

    public IActionResult Sala3(int clave)
    {
        if (!ValidarProgresoSala(3)) return RedirectToAction("Salas");
        
        string qteEstado = HttpContext.Session.GetString(QTE_TERMINADO_KEY);
        if (qteEstado == null)
        {
            HttpContext.Session.SetString(QTE_TERMINADO_KEY, "false");
            qteEstado = "false";
        }
        ViewBag.QteTerminado = qteEstado == "true";

        if (clave != 0)
        {
            if (qteEstado != "true")
            {
                return View();
            }

            if(new Sala3().Verificar(clave))
            {
                HttpContext.Session.SetString(SALA_KEY, "4");
                return RedirectToAction("Sala4");
            }
        }
        
        return View();
    }

    [HttpPost]
    public IActionResult CompletarQte()
    {
        HttpContext.Session.SetString(QTE_TERMINADO_KEY, "true");
        return Ok();
    }

    public IActionResult Sala4(int clave)
    {
        if (!ValidarProgresoSala(4)) return RedirectToAction("Salas");
        
        if (clave != 0)
        {
            if(new Sala4().Verificar(clave))
            {
                HttpContext.Session.SetString(SALA_KEY, "5");
                return RedirectToAction("Sala5");
            }
            return RedirectToAction("Derrota");
        }
        
        return View();
    }

    public IActionResult sala5(string wordle, int clave)
    {
        if (!ValidarProgresoSala(5)) return RedirectToAction("Salas");
        bool adivinoPalabra = false;
        int intentos = 0;
        if (intentos == 0)
        {
            Sala5 Sala5 = new Sala5();
            Sala5.crearRandom();
        } else{
            ViewBag.respuesta = Sala5.compararPalabra(wordle);
            intentos++;
            adivinoPalabra = Sala5.adivinaPalabra(wordle);
            if(adivinoPalabra){
                ViewBag.adivinoPalabra = adivinoPalabra;
            }
            if(Sala5.Verificar(clave)){
                HttpContext.Session.SetString(SALA_KEY, "6");
            }
        }
        return View();
    }


public IActionResult Sala6(char? letra, int? clave)
{
    if (!ValidarProgresoSala(6)) return RedirectToAction("Salas");

    var sala6 = ObjetoUtils.StringToObject<Sala6>(HttpContext.Session.GetString("Sala6")) ?? new Sala6();

    if (letra != null)
    {
        if (sala6.Jugar(letra.Value))
        {
            if (sala6.HaGanado())
            {
                return RedirectToAction("SalaFinal");
            }
            return RedirectToAction("Derrota");
        }
    }
    if (clave != null)
    {
        if (new Sala6().Verificar(clave.Value))
        {
            HttpContext.Session.SetString(SALA_KEY, "6");
            return RedirectToAction("Sala6");
        }
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
        if (HttpContext.Session.GetString(SALA_KEY) == "2")
        {
            HttpContext.Session.Remove(AHORCADO_KEY);
        }
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
  

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
            HttpContext.Session.SetString(SALA_KEY, "6");
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
        if (clave == 68735)return Redirect("https://www.youtube.com/watch?v=xvFZjo5PgG0&list=RDxvFZjo5PgG0&start_radio=1");
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

    private bool ValidarProgresoSala(int salaActual)
    {
        var salaMaxima = HttpContext.Session.GetString(SALA_KEY);
        if (string.IsNullOrEmpty(salaMaxima)) return false;
        return int.Parse(salaMaxima) >= salaActual;
    }
    [HttpGet]
    public IActionResult Sala5()
    {
        if (!ValidarProgresoSala(5)) return RedirectToAction("Salas");
        string? salaJson = HttpContext.Session.GetString("sala5");
        Sala5 sala = ObjetoUtils.StringToObject<Sala5>(salaJson) ?? new Sala5();
        if (string.IsNullOrEmpty(sala.palabraAdivinar))
            sala.crearRandom();
        if (sala.HistorialWordle.Count < sala.wordleActual)
            sala.HistorialWordle.Add(new salaEscape.Models.JugadaWordle { Palabra = sala.palabraAdivinar });
        HttpContext.Session.SetString("sala5", ObjetoUtils.ObjectToString(sala));
        ViewBag.HistorialWordle = sala.HistorialWordle;
        ViewBag.wordleActual = sala.wordleActual;
        ViewBag.palabraAdivinar = null;
        ViewBag.cantLetras = sala.palabraAdivinar != null ? sala.palabraAdivinar.Length : 0;
        ViewBag.finalizado = sala.wordleActual > 5;
        return View();
    }

    [HttpPost]
    [ActionName("Sala5")]
    public IActionResult Sala5Post(string? wordle, string? clave)
    {
        if (!ValidarProgresoSala(5)) return RedirectToAction("Salas");
        string? salaJson = HttpContext.Session.GetString("sala5");
        Sala5 sala = ObjetoUtils.StringToObject<Sala5>(salaJson) ?? new Sala5();
        if (string.IsNullOrEmpty(sala.palabraAdivinar))
            sala.crearRandom();
        bool adivino = false;
        if (!string.IsNullOrEmpty(wordle) && sala.wordleActual <= 5)
        {
            if (wordle.Length != sala.palabraAdivinar.Length)
            {
                ViewBag.respuesta = null;
                ViewBag.adivinoPalabra = false;
                ViewBag.error = $"La palabra debe tener {sala.palabraAdivinar.Length} letras.";
            }
            else
            {
                ViewBag.respuesta = sala.compararPalabra(wordle.ToLower());
                adivino = sala.adivinaPalabra(wordle.ToLower());
                ViewBag.adivinoPalabra = adivino;
                if (adivino)
                {
                    if (sala.wordleActual < 5)
                    {
                        sala.siguienteWordle();
                        ViewBag.mensaje = $"¡Correcto! Ahora resuelve el wordle {sala.wordleActual} de 5.";
                    }
                    else
                    {
                        sala.wordleActual++;
                        ViewBag.finalizado = true;
                    }
                }
            }
        }
        if (!string.IsNullOrEmpty(clave) && sala.wordleActual > 5)
        {
            int claveInt = 0;
            int.TryParse(clave, out claveInt);
            if (salaEscape.Models.Sala5.Verificar(claveInt))
            {
                HttpContext.Session.SetString(SALA_KEY, "6");
                return RedirectToAction("Sala6");
            }
            else
            {
                ViewBag.error = "Clave incorrecta.";
            }
        }
        HttpContext.Session.SetString("sala5", ObjetoUtils.ObjectToString(sala));
        ViewBag.HistorialWordle = sala.HistorialWordle;
        ViewBag.wordleActual = sala.wordleActual;
        ViewBag.palabraAdivinar = null;
        ViewBag.cantLetras = sala.palabraAdivinar != null ? sala.palabraAdivinar.Length : 0;
        ViewBag.finalizado = sala.wordleActual > 5;
        return View("Sala5");
    }
    bool creadaSala6 = false;
    [HttpGet]
    public IActionResult Sala6()
    {
        if (!ValidarProgresoSala(6)) return RedirectToAction("Salas");
        return View();
    }

    [HttpPost]
    public IActionResult Sala6(string? accion)
    {
        if (!ValidarProgresoSala(6)) return RedirectToAction("Salas");
        
        if (accion == "ganar")
        {
            return RedirectToAction("SalaFinal");
        }
        
        return RedirectToAction("Sala6");
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
  

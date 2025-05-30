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
    
        public IActionResult creditos()
    {
        return View();

    }
    public IActionResult salas()
    {
        return View();
        
    }
        public IActionResult sala1(int clave)
    {
        if(Sala1(clave)){
            string nuevaSalaMaxima = "2";
            HttpContext.Session.SetString("Sala", nuevaSalaMaxima);
        return View("sala2");}else{
            return View();
        }
    }
          public IActionResult sala2()
    {
        return View();

    }
         public IActionResult sala3(int clave)
    {
        
        if(Sala3(clave)){
            string nuevaSalaMaxima = "4";
            HttpContext.Session.SetString("Sala", nuevaSalaMaxima);
            return View("sala4");
        }else{
        return View();}

    }
           public IActionResult sala4()
    {
        return View();

    }
           public IActionResult sala5(int clave)
    {
        if(sala5(clave)){
            string nuevaSalaMaxima = "6";
            HttpContext.Session.SetString("Sala", nuevaSalaMaxima);
        return View("sala6");}else{
            return View();
        }
    }
           public IActionResult salaFinal()
    {
        return View();

    }
            public IActionResult historia()
    {
        return View();

    }
    
    public IActionResult sala6(char letra)
    {
        const int JUGADAS_MAXIMAS = 5;
var letrasRandom = ObjetoLista.ObtenerLista<char>(HttpContext.Session, "letrasRandom");
var letrasIngresadas = ObjetoLista.ObtenerLista<char>(HttpContext.Session, "letrasIngresadas");

ViewBag.LetrasRandom = letrasRandom;
ViewBag.LetrasIngresadas = letrasIngresadas;

return View();


        ViewBag.perdio = ObjetoUtils.StringToObject<bool>(HttpContext.Session.GetString("perdio"));
        ViewBag.cantJugadas = ObjetoUtils.StringToObject<int>(HttpContext.Session.GetString("jugadas"));
        
        if (ViewBag.perdio == true && ViewBag.jugadas == JUGADAS_MAXIMAS)
        {
            return View("salaFinal");
        }
        else if (ViewBag.perdio == true)
        {
           return View("derrota");

        }

        else{
            return View();
        } 
       
        


    }
}

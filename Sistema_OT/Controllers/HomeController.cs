using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;
using System.Diagnostics;

namespace Sistema_OT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string nroOt)
        {
            if (string.IsNullOrWhiteSpace(nroOt))
            {
                Console.WriteLine("Debes rellenar el formulario");
            }
            else
            {
                Console.WriteLine($"{nroOt}");
                string consulta = "select * from Ordenes_Trabajo where NroOrdenTrabajo = " + nroOt;
                List<OrdenDeTrabajo> ordenes = OrdenDeTrabajo.ObtenerLista(consulta);
                if (ordenes.Count > 0)
                {
                    ViewData["Orden"] = ordenes.First();
                }
                
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

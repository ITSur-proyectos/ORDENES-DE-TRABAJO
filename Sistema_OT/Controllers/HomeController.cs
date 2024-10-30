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
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Vistas()
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            return View();
        }
        [HttpPost]
        public IActionResult Vistas(int cliente, int sistema)
        {
            return View();
        }
        [HttpGet]
        public IActionResult PruebaBD()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PruebaBD(string nroOTD, string nroOTH)
        {
            if ((string.IsNullOrWhiteSpace(nroOTD)) || (string.IsNullOrWhiteSpace(nroOTH)))
            {
                Console.WriteLine("Debes rellenar el formulario");
            }
            else
            {
                if ((int.TryParse(nroOTD, out int nroOTDesde)) && (int.TryParse(nroOTH, out int nroOTHasta)))
                {
                    string consulta = "Ort_sp_OrdenesTrabajo_Listar2";
                    Dictionary<string, int> parametros = new Dictionary<string, int>();
                    parametros["@P_Cliente"] = nroOTDesde;
                    //parametros["@P_NroOrdenTrabajoHasta"] = nroOTHasta;

                    List<OrdenDeTrabajo> ordenes = OrdenDeTrabajo.ObtenerLista(consulta, parametros);
                    if (ordenes.Count > 0)
                    {
                        ViewData["Orden"] = ordenes;
                    }
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult VistaIndividual()
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
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

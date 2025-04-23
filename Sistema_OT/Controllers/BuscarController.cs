using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;
using System.Diagnostics;


namespace Sistema_OT.Controllers
{
    public class BuscarController : Controller
    {
        private readonly ILogger<BuscarController> _logger;

        public BuscarController(ILogger<BuscarController> logger)
        {
            _logger = logger;
        }

        public IActionResult VistaIndividualBusca()
        {
            // Aquí puedes hacer cualquier lógica que necesites antes de mostrar la vista
            return View("~/Views/Shared/VistaIndividualBuscar.cshtml");
            // Esto buscará la vista VistaIndividualBusca.cshtml en la carpeta Views/Buscar
        }

        public IActionResult Index()
        {
            // Verificar si el usuario está logueado (por ejemplo, comprobando la sesión)
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account"); // Redirige a la vista de login si no está logueado
            }
            return View();
        }
        [HttpGet]
        public IActionResult Vistas()
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            return View();
        }

        [HttpPost]
       
         public IActionResult Vistas(int Cliente, int Sistema, int estadoTrabajo, string usuarioSolicitante, string Responsable, string asunto, string modulo, int Proyecto, DateTime? fechaSolicitudDesde, DateTime? fechaSolicitudHasta, DateTime? fechaVencimientoDesde, DateTime? fechaVencimientoHasta)
         {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            Dictionary<string, object> parametros = new Dictionary<string, object>();

            // Añade el valor a los parámetros de la sp si se ingresó
            if (Cliente != -1)
            {
                parametros["@Cliente"] = Cliente;
            }
            if (Sistema != -1)
            {
                parametros["@Sistema"] = Sistema;
            }
            if (Proyecto != -1)
            {
                parametros["@Proyecto"] = Proyecto;
            }
            if (estadoTrabajo != 0)
            {
                parametros["@Estado"] = estadoTrabajo;
            }
            if (!string.IsNullOrWhiteSpace(usuarioSolicitante))
            {
                parametros["@UsuarioSolicitante"] = usuarioSolicitante;
            }
            if (!string.IsNullOrWhiteSpace(Responsable))
            {
                parametros["@UserIDResponsable"] = Responsable;
            }
            if (!string.IsNullOrWhiteSpace(asunto))
            {
                parametros["@Asunto"] = asunto;
            }
            if (!string.IsNullOrWhiteSpace(modulo))
            {
                parametros["@Modulo"] = modulo;
            }
            // Agrega los parámetros de fecha solo si tienen valor
            if (fechaSolicitudDesde.HasValue)
            {
                parametros["@FechaDesde"] = fechaSolicitudDesde.Value;
            }
            if (fechaSolicitudHasta.HasValue)
            {
                parametros["@FechaHasta"] = fechaSolicitudHasta.Value;
            }
            if (fechaVencimientoDesde.HasValue)
            {
                parametros["@FechaVencimientoDesde"] = fechaVencimientoDesde.Value;
            }
            if (fechaVencimientoHasta.HasValue)
            {
                parametros["@FechaVencimientoHasta"] = fechaVencimientoHasta.Value;
            }

            // Hacer la consulta si se ingresó al menos 1 parámetro
            if (parametros.Count > 0)
            {
                string consulta = "sp_BuscarOrdenesTrabajo";
                List<Dictionary<string, object>> ordenes = OrdenDeTrabajo.ObtenerLista(consulta, parametros);

                if (ordenes.Count > 0)
                {
                    ViewData["Ordenes"] = ordenes;
                }
            }
            else
            {
                Console.WriteLine("No llenaste los formularios.");
            }


            return View();
         }
    
        [HttpGet]
        public IActionResult PruebaBD()
        {
            return View();
        }


        [HttpGet]
        public IActionResult VistaIndividualBuscar(string activeSection = "descripcion")
        {
            ViewBag.ActiveSection = activeSection;
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            return View();
        }
        [HttpPost]
        public IActionResult VistaIndividualBuscar(int orden)
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros["@NroOrdenTrabajo"] = orden;
            // Hacer la consulta si se ingresó parametro
            if (parametros.Count > 0)
            {
                string consulta = "sp_ConsultarOrdenTrabajoIndividual";
                List<Dictionary<string, object>> ordenes = OrdenDeTrabajo.ObtenerLista(consulta, parametros);

                if (ordenes.Count > 0)
                {
                    ViewData["Orden"] = ordenes;
                }
            }
            else
            {
                Console.WriteLine("No llenaste los formularios.");
            }
            return View();
        }

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
                    Dictionary<string, object> parametros = new Dictionary<string, object>();
                    parametros["@P_Cliente"] = nroOTDesde;
                    //parametros["@P_NroOrdenTrabajoHasta"] = nroOTHasta;

                    List<Dictionary<string, object>> ordenes = OrdenDeTrabajo.ObtenerLista(consulta, parametros);
                    if (ordenes.Count > 0)
                    {
                        ViewData["Orden"] = ordenes;
                    }
                }

            }
            return View();
        }

        [HttpGet]
        

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




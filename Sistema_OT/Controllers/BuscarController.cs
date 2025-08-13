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
            // Mapeo cliente -> sistemas y sistema -> usuario responsable
            ViewData["SistemasPorCliente"] = OrdenDeTrabajo.ConseguirSistemasPorCliente(); // Dictionary<int, List<int>>
            ViewData["UsuariosResponsablesPorSistema"] = OrdenDeTrabajo.ConseguirUsuarioResponsablePorSistema(); // Dictionary<int, int>

            return View();
        }
        [HttpPost]
        
         public IActionResult Vistas(int Cliente, int Sistema, int estadoTrabajo, string usuarioSolicitante, string Responsable, string asunto, string modulo, int Proyecto, DateTime? fechaSolicitudDesde, DateTime? fechaSolicitudHasta, DateTime? fechaVencimientoDesde, DateTime? fechaVencimientoHasta, string mod)
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");


            ViewBag.Cliente = Cliente;
            ViewBag.Sistema = Sistema;
            ViewBag.Proyecto = Proyecto;
            ViewBag.EstadoTrabajo = estadoTrabajo;
            ViewBag.UsuarioSolicitante = usuarioSolicitante;
            ViewBag.Responsable = Responsable;
            ViewBag.Asunto = asunto;
            ViewBag.Modulo = modulo;
            ViewBag.FechaSolicitudDesde = fechaSolicitudDesde;
            ViewBag.FechaSolicitudHasta = fechaSolicitudHasta;
            ViewBag.FechaVencimientoDesde = fechaVencimientoDesde;
            ViewBag.FechaVencimientoHasta = fechaVencimientoHasta;
            ViewBag.Mod = mod;

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
            if (!string.IsNullOrWhiteSpace(mod))
            {
                parametros["@ModificacionesBaseDatos"] = mod;
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
            ViewBag.Asunto = asunto;
            ViewBag.FechaSolicitudDesde = fechaSolicitudDesde?.ToString("yyyy-MM-dd");
            ViewBag.FechaSolicitudHasta = fechaSolicitudHasta?.ToString("yyyy-MM-dd");
            ViewBag.FechaVencimientoDesde = fechaVencimientoDesde?.ToString("yyyy-MM-dd");
            ViewBag.FechaVencimientoHasta = fechaVencimientoHasta?.ToString("yyyy-MM-dd");

            return View();
         }
    
        [HttpGet]
        public IActionResult PruebaBD()
        {
            return View();
        }


        [HttpPost]
        public IActionResult VistaIndividualBuscar(int orden)
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            ViewData["Avances_Trabajo"] = AvancesTrabajoModel.ConseguirAvances(orden);
            ViewData["HistorialEstados"] = HistorialdeEstadoModel.ConseguirHistorial(orden);
            ViewData["Adjuntos"] = ArchivoAdjuntoModel.ConseguirAdjuntos(orden);


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


        [HttpGet]
        public IActionResult VistaIndividualBuscar(string activeSection = "descripcion")
        {
            ViewBag.ActiveSection = activeSection;
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            // Mapeo cliente -> sistemas
            ViewData["SistemasPorCliente"] = OrdenDeTrabajo.ConseguirSistemasPorCliente(); // Dictionary<int, List<int>>
            // Mapeo sistema -> usuario responsable
            ViewData["UsuariosResponsablesPorSistema"] = OrdenDeTrabajo.ConseguirUsuarioResponsablePorSistema(); // Dictionary<int, int>

            return View();
        }


        public static string ConvertirRtfATextoPlano(string rtf)
        {
            if (string.IsNullOrWhiteSpace(rtf))
                return string.Empty;

            // Reemplazar etiquetas RTF básicas con texto plano
            var cleanText = rtf;
            cleanText = cleanText.Replace(@"{\rtf1", string.Empty);  // Eliminar el encabezado RTF
            cleanText = cleanText.Replace(@"\par", Environment.NewLine);  // Reemplazar saltos de línea
            cleanText = cleanText.Replace(@"\fs24", string.Empty);  // Eliminar tamaño de fuente
            cleanText = cleanText.Replace(@"\ansi", string.Empty);  // Eliminar configuraciones de fuente

            // Si hay más patrones que quieras eliminar, puedes agregar más reemplazos aquí

            return cleanText;
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




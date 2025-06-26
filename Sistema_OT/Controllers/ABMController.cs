using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using static Sistema_OT.Models.AvancesTrabajoModel;
using Sistema_OT.ViewModels;
using RtfPipe.Tokens;
using System.Diagnostics;
using System.Net;

namespace Sistema_OT.Controllers
{
    public class ABMController : Controller
    {

        [HttpGet]
        public ActionResult VistaIndividual()
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            ViewData["UsuarioLogueado"] = HttpContext.Session.GetString("UserId");

            //ViewData["NombresSistemas_Cliente"] = OrdenDeTrabajo.ConseguirNombres("Sistemas_Cliente"); //EUGE1
            //ViewData["SistemasPorCliente"] = OrdenDeTrabajo.ConseguirSistemasPorCliente();

            //string usuarioLogueado = HttpContext.Session.GetString("UserId") ?? "";

            //var nombresUsuarios = OrdenDeTrabajo.ConseguirNombres("Usuario");
            //ViewBag.NombresUsuarios = nombresUsuarios;
            //ViewBag.UsuarioLogueado = usuarioLogueado;

            return View();
        }



        [HttpPost]
        public ActionResult VistaIndividualBuscar(string accion, int Cliente, int Sistema, float cantidadHorasEstimada, int estadoTrabajo, string usuarioSolicitante, string Responsable, string asunto, string modulo, int Proyecto, DateTime? fechaSolicitud, DateTime? fechaVencimiento, char premioAvance, char alcanceIndefinido, string descripcion)
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");

            if (accion == "Grabar")
            {
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
                    parametros["@UserIDSolicitante"] = usuarioSolicitante;
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
                if (fechaSolicitud.HasValue)
                {
                    parametros["@FechaSolicitud"] = fechaSolicitud.Value;
                }
                if (fechaVencimiento.HasValue)
                {
                    parametros["@FechaVencimiento"] = fechaVencimiento.Value;
                }
                // Agregar descripción sin limpieza, solo si tiene texto
                if (!string.IsNullOrWhiteSpace(descripcion))
                {
                    parametros["@Descripcion"] = descripcion;
                }

                if (parametros.Count > 0)
                {
                    parametros["@PremioPorAvance"] = premioAvance;
                    parametros["@AlcanceIndefinido"] = alcanceIndefinido;
                    string consulta = "Ordenes_Trabajo_INSERT";

                    int nroOrden;

                    int resultado = OrdenDeTrabajo.EjecutarInsert(consulta, parametros, out nroOrden);

                    if (resultado > 0 && nroOrden > 0)
                    {
                        TempData["Mensaje"] = "¡Registro creado con éxito!";
                        // REDIRECCIÓN A OTRA ACCIÓN EN OTRO CONTROLADOR:
                        return RedirectToAction("VistaIndividualModificar", new { orden = nroOrden });

                    }
                    else
                    {
                        ViewBag.Error = "No se pudo insertar la orden.";
                    }
                }
                else
                {
                    Console.WriteLine("No llenaste los formularios.");
                }
            }
            return View();
        }


        public IActionResult VistaIndividualModificar(int orden)
        {
            // Cargar catálogos para selects y listas auxiliares
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");
            // Cargar las secciones relacionadas con la OT
            ViewData["Avances_Trabajo"] = AvancesTrabajoModel.ConseguirAvances(orden);
            ViewData["HistorialEstados"] = HistorialdeEstadoModel.ConseguirHistorial(orden);
            ViewData["Estados"] = HistorialdeEstadoModel.ConseguirEstados();
            ViewData["Transiciones"] = HistorialdeEstadoModel.Transiciones();
            ViewData["Adjuntos"] = ArchivoAdjuntoModel.ConseguirAdjuntos(orden);
            ViewData["UsuarioLogueado"] = HttpContext.Session.GetString("UserId");

            // Cargar la orden de trabajo individual con todos los datos (igual que en Buscar)
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            

            parametros["@NroOrdenTrabajo"] = orden;

            if (parametros.Count > 0)
            {
                string consulta = "sp_ConsultarOrdenTrabajoIndividual";
                List<Dictionary<string, object>> ordenes = OrdenDeTrabajo.ObtenerLista(consulta, parametros);

                if (ordenes.Count > 0)
                {
                    ViewData["Orden"] = ordenes;
                }
            }

            ViewBag.NroOrdenTrabajo = orden;

            return View();
        }

 

        [HttpPost]
        public ActionResult ActualizarOrden( 
            int NroOrdenTrabajo,
            string depende = null,
            DateTime? FechaSolicitud = null,
            DateTime? FechaVencimiento = null,
            int? EstadoDescripcion = null,
            int? EstadoActual = null,
            string ClienteNombre = null,
            string SistemaNombre = null,
            string ProyectoNombre = null,
            string ResponsableNombre = null,
            string SolicitanteNombre = null,
            string SolicitadoPorNombre = null,
            string Modulo = null,
            string Asunto = null,
            string Descripcion = null,
            int? PorcentajeAvance = null,
            int? CantidadHorasConsumidas = null,
            int? NroOtImplementacion = null,
            char PremioPorAvance = 'N',
            char AlcanceIndefinido = 'N',
            string ModificacionesBaseDatos = null,
            string FormulariosModificados = null,
            List<AvancesTrabajoModel> Avances = null  // Lista de avances nuevos que llegan del form
        )
        {
            try
            {
                var usuarioLogueado = HttpContext.Session.GetString("UserId");

                int estadoFinal = 0;
                if (!EstadoDescripcion.HasValue || EstadoDescripcion == 0)
                {
                    estadoFinal = EstadoActual ?? 0;  // si EstadoActual también es null, queda 0
                }
                else
                {
                    estadoFinal = EstadoDescripcion.Value;
                }
                //para avances
                if (Avances != null && Avances.Any())
                {
                    foreach (var avance in Avances)
                    {
                        avance.NroOrdenTrabajo = NroOrdenTrabajo; // asigno el nro de orden a cada avance
                        avance.UserIDAlta = usuarioLogueado;       // asignar usuario si no llega desde el form
                                                                   // Guarda o procesa cada avance
                        AvancesTrabajoModel.GuardarAvance(avance);
                    }
                }

                //orden 
                var orden = new OrdenDeTrabajo
                {
                    NroOrdenTrabajo = NroOrdenTrabajo,
                    DependeDe = !string.IsNullOrEmpty(depende) ? int.Parse(depende) : 0,
                    FechaSolicitud = FechaSolicitud ?? DateTime.MinValue,
                    //FechaFinalizacion = FechaVencimiento ?? DateTime.MinValue,
                    Estado = estadoFinal,
                    Cliente = ClienteNombre,
                    Sistema = SistemaNombre,
                    Proyecto = ProyectoNombre,
                    UsuarioResponsable = ResponsableNombre,
                    UsuarioSolicitante = SolicitanteNombre,
                    UserIDResponsable = ResponsableNombre,
                    UserIDSolicitante = SolicitanteNombre,
                    Modulo = Modulo,
                    Asunto = Asunto,
                    Descripcion = Descripcion,
                    PorcentajeAvance = PorcentajeAvance ?? 0,
                    CantidadHorasConsumidas = CantidadHorasConsumidas ?? 0,
                    // NroOtImplementacion aún no está en el modelo, si se agrega hacerlo aquí también
                    // Checkbox en forma de char (N/S)
                    // Convertimos char a bool internamente si hace falta en la capa de persistencia
                    UsuarioQueModifico = usuarioLogueado,
                    ModificacionesBaseDatos = ModificacionesBaseDatos,
                    FormulariosModificados = FormulariosModificados
                };
           

                OrdenDeTrabajo.Actualizar(orden);

                TempData["MensajeExito"] = "La orden se actualizó correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Ocurrió un error: " + ex.Message;
            }

            return RedirectToAction("VistaIndividualModificar", new { orden = NroOrdenTrabajo });
        }




        [HttpPost]
        public ActionResult GrabarOrden(OrdenConAvancesViewModel model)
        {
            // Guardar orden principal
            //OrdenDeTrabajo.Guardar(model.Orden); // ← corregido: modelo es OrdenDeTrabajo

            //foreach (var avance in model.Avances)
            //{
            //    avance.NroOrdenTrabajo = (int)model.Orden.NroOrdenTrabajo;

            //    avance.UserIDAlta ??= User.Identity.Name;
            //    avance.Fecha = avance.Fecha == default ? DateTime.Now : avance.Fecha;

            //    //AvancesTrabajoModel.Guardar(avance); // ← método dentro del modelo
            //}

            return RedirectToAction("Index");
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








    }

}

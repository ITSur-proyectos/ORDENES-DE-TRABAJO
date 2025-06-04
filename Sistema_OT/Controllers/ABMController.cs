using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;
using Microsoft.AspNetCore.Authorization;

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
            //ViewData["NombresSistemas_Cliente"] = OrdenDeTrabajo.ConseguirNombres("Sistemas_Cliente"); //EUGE1
            //ViewData["SistemasPorCliente"] = OrdenDeTrabajo.ConseguirSistemasPorCliente();

            //string usuarioLogueado = HttpContext.Session.GetString("UserId") ?? "";

            //var nombresUsuarios = OrdenDeTrabajo.ConseguirNombres("Usuario");
            //ViewBag.NombresUsuarios = nombresUsuarios;
            //ViewBag.UsuarioLogueado = usuarioLogueado;



            return View();
        }



        [HttpPost]
        public ActionResult VistaIndividualBuscar(string accion, int Cliente, int Sistema, float cantidadHorasEstimada, int estadoTrabajo, string usuarioSolicitante, string Responsable, string asunto, string modulo, int Proyecto, DateTime? fechaSolicitud, DateTime? fechaVencimiento, char premioAvance, char alcanceIndefinido)
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
                        return RedirectToAction("EditarOrden", "Buscar", new { orden = nroOrden });
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





        /**************************EDICION**********************************/


        [HttpGet]
        public IActionResult EditarOrden(int orden)
        {
            // Cargar datos auxiliares
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");

            // Cargar la OT específica
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros["@NroOrdenTrabajo"] = orden;

            string consulta = "sp_ConsultarOrdenTrabajoIndividual";
            var ordenes = OrdenDeTrabajo.ObtenerLista(consulta, parametros);

            if (ordenes.Count > 0)
            {
                ViewData["Orden"] = ordenes[0]; // o pasalo como modelo si querés strongly typed
                return View();
            }

            TempData["Error"] = "No se encontró la orden.";
            return RedirectToAction("VistaOrdenes"); // o la que uses como listado general
        }

        //[HttpPost]
        //public IActionResult EditarOrden(int NroOrdenTrabajo, string Asunto, string Modulo /* + otros campos */)
        //{
        //    Dictionary<string, object> parametros = new Dictionary<string, object>
        //    {
        //        ["@NroOrdenTrabajo"] = NroOrdenTrabajo,
        //        ["@Asunto"] = Asunto,
        //        ["@Modulo"] = Modulo
        //        // + otros campos
        //    };

        //    string sp = "Ordenes_Trabajo_UPDATE";
        //  //  int resultado = OrdenDeTrabajo.EjecutarUpdate(sp, parametros);

        //    //if (resultado > 0)
        //    //{
        //    //    TempData["Mensaje"] = "Orden actualizada correctamente.";
        //    //    return RedirectToAction("VistaIndividualBuscar", new { orden = NroOrdenTrabajo });
        //    //}

        //    TempData["Error"] = "Error al actualizar la orden.";
        //    return RedirectToAction("EditarOrden", new { orden = NroOrdenTrabajo });
        //}


    }

}

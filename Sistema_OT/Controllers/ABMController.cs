using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;

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
            return View();
        }
        [HttpPost]
        public ActionResult VistaIndividual(string accion, int Cliente, int Sistema, int estadoTrabajo, string usuarioSolicitante, string Responsable, string asunto, string modulo, int Proyecto, DateTime? fechaSolicitud)
        {
            ViewData["NombresUsuarios"] = OrdenDeTrabajo.ConseguirNombres("Usuario");
            ViewData["NombresSistemas"] = OrdenDeTrabajo.ConseguirNombres("Sistema");
            ViewData["NombresClientes"] = OrdenDeTrabajo.ConseguirNombres("Cliente");
            ViewData["NombresProyectos"] = OrdenDeTrabajo.ConseguirNombres("Proyecto");

            if (accion == "Agregar")
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
                if (parametros.Count > 0)
                {
                    string consulta = "Ordenes_Trabajo_INSERT";
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
            }
           

            return View();
        }

    }

}

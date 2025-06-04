using Microsoft.AspNetCore.Mvc;
using Sistema_OT.Models;

public class AvancesController : Controller
{
    // Acción que muestra la lista de avances
    public IActionResult Index(int nroOrden)
    {
        var listaAvances = AvancesTrabajoModel.ConseguirAvances(nroOrden);
        return View(listaAvances);
    }

    //// Acción para mostrar el formulario de alta (crear)
    //public IActionResult Crear(int nroOrden)
    //{
    //    return View(new AvancesTrabajoModel()); // Pasa un modelo vacío para el formulario
    //}

    //// Acción para procesar el alta (crear)
    //[HttpPost]
    //public IActionResult Crear(AvancesTrabajoModel modelo)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        // Llamar a un método de la clase modelo para guardar en la base de datos
    //        AvancesTrabajoModel.CrearAvance(modelo);
    //        return RedirectToAction("Index", new { nroOrden = modelo.NroOrdenTrabajo });
    //    }
    //    return View(modelo);
    //}

    //// Acción para mostrar el formulario de edición
    //public IActionResult Editar(int id)
    //{
    //    var avance = AvancesTrabajoModel.ConseguirAvancePorId(id);
    //    if (avance == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(avance);
    //}

    //// Acción para procesar la edición
    //[HttpPost]
    //public IActionResult Editar(AvancesTrabajoModel modelo)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        // Llamar al método de la clase modelo para actualizar el avance
    //        AvancesTrabajoModel.EditarAvance(modelo);
    //        return RedirectToAction("Index", new { nroOrden = modelo.NroOrdenTrabajo });
    //    }
    //    return View(modelo);
    //}

    //// Acción para eliminar un avance
    //[HttpPost]
    //public IActionResult Eliminar(int id)
    //{
    //    var avance = AvancesTrabajoModel.ConseguirAvancePorId(id);
    //    if (avance != null)
    //    {
    //        // Llamar al método de la clase modelo para eliminar el avance
    //        AvancesTrabajoModel.EliminarAvance(id);
    //    }
    //    return RedirectToAction("Index");
    //}
}

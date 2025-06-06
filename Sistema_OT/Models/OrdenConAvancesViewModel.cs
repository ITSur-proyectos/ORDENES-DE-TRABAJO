using Sistema_OT.Models;
using System.Collections.Generic;

namespace Sistema_OT.ViewModels
{
    public class OrdenConAvancesViewModel
    {
        public OrdenDeTrabajo Orden { get; set; } // Reemplazá con tu modelo real
        public List<AvancesTrabajoModel> Avances { get; set; } = new List<AvancesTrabajoModel>();
    }
}

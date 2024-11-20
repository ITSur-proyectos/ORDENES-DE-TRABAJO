namespace Sistema_OT.Models
{
    public class AvanceDeTrabajo
    {
        public decimal NroOrdenTrabajo { get; set; }

        public int AvanceTrabajo { get; set; }

        public string Descripcion { get; set; }

        public DateTime Fecha { get; set; }

        public decimal HorasInsumidas { get; set; }

        public char Terminado { get; set; }

        public DateTime FechaAlta { get; set; }

        public string UserIDAlta { get; set; }

        public DateTime FechaUltimaModificacion { get; set; }

        public string UserIDModificacion { get; set; }

        public DateTime FechaBaja { get; set; }

        public string UserIDBaja { get; set; }

        public int Usuario { get; set; }

        public int TipoAvance { get; set; }
    }
}

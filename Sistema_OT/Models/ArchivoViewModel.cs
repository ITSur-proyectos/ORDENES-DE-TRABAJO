namespace Sistema_OT.Models
{
    public class ArchivoViewModel
    {
       
            public int Id { get; set; }
            public string NombreArchivo { get; set; }
            public string TipoContenido { get; set; }
            public byte[] ArchivoData { get; set; }
            public DateTime FechaSubida { get; set; }
            public int NroOrdenTrabajo { get; set; }
            public int CorrelativoAdjunto { get; set; }
            public DateTime FechaAlta { get; set; }
            public string UserID { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
            public string UserIDModificacion { get; set; }
            public DateTime? FechaBaja { get; set; }
            public string UserIDBaja { get; set; }


      
    }
}

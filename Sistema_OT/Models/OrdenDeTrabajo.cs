using Sistema_OT.Services;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace Sistema_OT.Models
{
    public class OrdenDeTrabajo
    {

        
        public decimal NroOrdenTrabajo { get; set; }
        public int Cliente { get; set; }
        public int Sistema { get; set; }
        public string Modulo {  get; set; }
        public string Asunto { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public int CantidadHorasEstimadas {  get; set; }
        public int CantidadHorasConsumidas {  get; set; }
        public int Estado {  get; set; }
        public int PorcentajeAvance {  get; set; }
        public int UsuarioSolicitante {  get; set; }
        public int UsuarioResponsable {  get; set; }
        public string Descripcion {  get; set; }
        public string Observaciones { get; set; }
        public int Prioridad { get; set; }
        public string FormulariosModificados { get; set; }
        public string ModificacionesBaseDatos { get; set; }
        public string UserIDSolicitante { get; set; }
        public string UserIDResponsable { get; set; }
        public static List<OrdenDeTrabajo> ObtenerLista(int num)
        {
            string consulta = "Select * from Ordenes_Trabajo Where NroOrdenTrabajo = @numero";
            List<OrdenDeTrabajo> OrdenesTrabajo = new List<OrdenDeTrabajo>();
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();
            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))

            {
                command.Parameters.AddWithValue("@numero", num);
                
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrdenDeTrabajo Orden = new OrdenDeTrabajo
                            {

                                
                                NroOrdenTrabajo = reader.GetDecimal(reader.GetOrdinal("NroOrdenTrabajo")),
                                Cliente = reader.GetInt32(reader.GetOrdinal("Cliente")),
                                Sistema = reader.GetInt32(reader.GetOrdinal("Sistema")),
                                Modulo = reader.GetString(reader.GetOrdinal("Modulo")),
                                Asunto = reader.GetString(reader.GetOrdinal("Asunto")),
                                FechaSolicitud = reader.GetDateTime(reader.GetOrdinal("FechaSolicitud")),
                                FechaFinalizacion = reader.GetDateTime(reader.GetOrdinal("FechaFinalizacion")),
                                CantidadHorasEstimadas = reader.GetInt32(reader.GetOrdinal("CantidadHorasEstimadas")),
                                CantidadHorasConsumidas = reader.GetInt32(reader.GetOrdinal("CantidadHorasConsumidas")),
                                Estado = reader.GetInt32(reader.GetOrdinal("Estado")),
                                PorcentajeAvance = reader.GetInt32(reader.GetOrdinal("PorcentajeAvance")),
                                UsuarioSolicitante = reader.GetInt32(reader.GetOrdinal("UsuarioSolicitante")),
                                UsuarioResponsable = reader.GetInt32(reader.GetOrdinal("UsuarioResponsable")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Observaciones = reader.GetString(reader.GetOrdinal("Observaciones")),
                                Prioridad = reader.GetInt32(reader.GetOrdinal("Prioridad")),
                                FormulariosModificados = reader.GetString(reader.GetOrdinal("FormulariosModificados")),
                                ModificacionesBaseDatos = reader.GetString(reader.GetOrdinal("ModificacionesBaseDatos")),
                                UserIDSolicitante = reader.GetString(reader.GetOrdinal("UserIDSolicitante")),
                                UserIDResponsable = reader.GetString(reader.GetOrdinal("UserIDResponsable"))
                            };
                            OrdenesTrabajo.Add(Orden);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Hubo un error al ejecutar la consulta: " + e.ToString());
                }
            }
            return OrdenesTrabajo;
        }
    }

}

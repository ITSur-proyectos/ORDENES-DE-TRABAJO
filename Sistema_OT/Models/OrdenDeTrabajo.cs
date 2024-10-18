using Sistema_OT.Services;
using System.Data.SqlClient;

namespace Sistema_OT.Models
{
    public class OrdenDeTrabajo
    {
        public decimal NroOrdenTrabajo { get; set; }
        public int Cliente { get; set; }
        public int Sistema { get; set; }
        public string Asunto { get; set; }
        public DateTime FechaSolicitud { set; get; }
        public string UserIDSolicitante { get; set; }

        public static List<OrdenDeTrabajo> ObtenerLista(string consulta)
        {

            List<OrdenDeTrabajo> OrdenesTrabajo = new List<OrdenDeTrabajo>();
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();
            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))

            {
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
                                Asunto = reader.GetString(reader.GetOrdinal("Asunto")),
                                FechaSolicitud = reader.GetDateTime(reader.GetOrdinal("FechaSolicitud")),
                                UserIDSolicitante = reader.GetString(reader.GetOrdinal("UserIDSolicitante"))
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

using Sistema_OT.Services;
using System.Data.SqlClient;

namespace Sistema_OT.Models
{
    public class HistorialdeEstadoModel
    {
        public int Secuencia { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UserID { get; set; }

        public static List<HistorialdeEstadoModel> ConseguirHistorial(int nroOrden)
        {
            List<HistorialdeEstadoModel> historial = new List<HistorialdeEstadoModel>();
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();

            string consulta = @"
        SELECT Secuencia,  Descripcion, HEOT.FechaAlta, HEOT.UserID
        FROM Historial_Estados_OrdenesTrabajo HEOT
        INNER JOIN Estados_Trabajo ET ON ET.Estado = HEOT.Estado
        WHERE NroOrdenTrabajo = @NroOrdenTrabajo";

            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))
            {
                command.Parameters.AddWithValue("@NroOrdenTrabajo", nroOrden);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No se encontraron registros para el NroOrdenTrabajo: " + nroOrden);
                        }

                        while (reader.Read())
                        {
                            var item = new HistorialdeEstadoModel
                            {
                                Secuencia = reader.GetInt32(reader.GetOrdinal("Secuencia")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                                UserID = reader.GetString(reader.GetOrdinal("UserID"))
                            };
                            historial.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return historial;
        }

    }
}

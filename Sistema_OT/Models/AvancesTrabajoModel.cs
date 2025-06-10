using Sistema_OT.Services;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;

namespace Sistema_OT.Models
{
    public class AvancesTrabajoModel
    {
        public int AvanceTrabajoId { get; set; }
        public int NroOrdenTrabajo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasInsumidas { get; set; }
        public string UserIDAlta { get; set; }
        public string TipoAvance { get; set; }



        public static List<AvancesTrabajoModel> ConseguirAvances(int nroOrden)

        {
            List<AvancesTrabajoModel> listaAvances = new List<AvancesTrabajoModel>();

            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();

            string consulta = @"
            SELECT *
            FROM Avances_Trabajo
            WHERE NroOrdenTrabajo = @NroOrdenTrabajo";

            using (SqlCommand command = new SqlCommand(consulta, conexionDB.con))
            {
                command.Parameters.AddWithValue("@NroOrdenTrabajo", nroOrden);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var avance = new AvancesTrabajoModel
                            {
                                AvanceTrabajoId = reader.GetInt32(reader.GetOrdinal("AvanceTrabajo")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                HorasInsumidas = reader.GetDecimal(reader.GetOrdinal("HorasInsumidas")),
                                UserIDAlta = reader.IsDBNull(reader.GetOrdinal("UserIDAlta")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserIDAlta"))
                            };


                            listaAvances.Add(avance);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return listaAvances;
        }

        //INSERT GUARDA LO NUEVO 
        public static void Guardar(AvancesTrabajoModel avance)
        {
            ConexionDB conexion = new ConexionDB();
            conexion.AbrirConexion();

            string query = @"
        INSERT INTO Avances_Trabajo
        (NroOrdenTrabajo, Descripcion, Fecha, HorasInsumidas, UserIDAlta, TipoAvance)
        VALUES
        (@NroOrdenTrabajo, @Descripcion, @Fecha, @HorasInsumidas, @UserIDAlta, @TipoAvance)";

            using (SqlCommand cmd = new SqlCommand(query, conexion.con))
            {
                cmd.Parameters.AddWithValue("@NroOrdenTrabajo", avance.NroOrdenTrabajo);
                cmd.Parameters.AddWithValue("@Descripcion", avance.Descripcion ?? "");
                cmd.Parameters.AddWithValue("@Fecha", avance.Fecha);
                cmd.Parameters.AddWithValue("@HorasInsumidas", avance.HorasInsumidas);
                cmd.Parameters.AddWithValue("@UserIDAlta", avance.UserIDAlta ?? "");
                cmd.Parameters.AddWithValue("@TipoAvance", avance.TipoAvance ?? "");

                cmd.ExecuteNonQuery();
            }
        }



    }
}

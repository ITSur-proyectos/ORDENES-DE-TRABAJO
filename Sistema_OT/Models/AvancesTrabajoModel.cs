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
        public string descripcionTipoAvance { get; set; }



        public static List<AvancesTrabajoModel> ConseguirAvances(int nroOrden)

        {
            List<AvancesTrabajoModel> listaAvances = new List<AvancesTrabajoModel>();

            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();

            string consulta = @"
             SELECT A.*, TA.Descripcion AS descripcionTipoAvance
            FROM Avances_Trabajo A 
			inner join Tipos_Avances TA ON TA.TipoAvance = A.TipoAvance
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
                                descripcionTipoAvance = reader.IsDBNull(reader.GetOrdinal("descripcionTipoAvance")) ? string.Empty : reader.GetString(reader.GetOrdinal("descripcionTipoAvance")),
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


        public static void GuardarAvance(AvancesTrabajoModel avance)
        {
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();

            // Obtener el próximo AvanceTrabajoId para esta OT
            string getMaxQuery = @"
        SELECT ISNULL(MAX(AvanceTrabajo), 0) + 1
        FROM Avances_Trabajo
        WHERE NroOrdenTrabajo = @NroOrdenTrabajo";

            int nuevoAvanceId = 1;

            using (SqlCommand getMaxCmd = new SqlCommand(getMaxQuery, conexionDB.con))
            {
                getMaxCmd.Parameters.AddWithValue("@NroOrdenTrabajo", avance.NroOrdenTrabajo);
                nuevoAvanceId = (int)getMaxCmd.ExecuteScalar();
            }

            string insertQuery = @"
        INSERT INTO Avances_Trabajo (
            NroOrdenTrabajo,
            AvanceTrabajo,
            Descripcion,
            Fecha,
            HorasInsumidas,
            FechaAlta,
            UserIDAlta,
            TipoAvance
        )
        VALUES (
            @NroOrdenTrabajo,
            @AvanceTrabajo,
            @Descripcion,
            @Fecha,
            @HorasInsumidas,
            GETDATE(),
            @UserIDAlta,
            @TipoAvance
        )";

            using (SqlCommand cmd = new SqlCommand(insertQuery, conexionDB.con))
            {
                cmd.Parameters.AddWithValue("@NroOrdenTrabajo", avance.NroOrdenTrabajo);
                cmd.Parameters.AddWithValue("@AvanceTrabajo", nuevoAvanceId);
                cmd.Parameters.AddWithValue("@Descripcion", avance.Descripcion ?? "");
                cmd.Parameters.AddWithValue("@Fecha", avance.Fecha);
                cmd.Parameters.AddWithValue("@HorasInsumidas", avance.HorasInsumidas);
                cmd.Parameters.AddWithValue("@UserIDAlta", avance.UserIDAlta ?? ""); // ejemplo: 'ERUIZ'
                ////cmd.Parameters.AddWithValue("@TipoAvance", avance.TipoAvance ?? "9");
                //cmd.Parameters.AddWithValue("@TipoAvance", string.IsNullOrEmpty(avance.TipoAvance) ? "9" : avance.TipoAvance);

                cmd.Parameters.AddWithValue("@TipoAvance", avance.TipoAvance);
                cmd.ExecuteNonQuery();
            }
        }




        //INSERT GUARDA LO NUEVO 
        //public static void Guardar(AvancesTrabajoModel avance)
        //{
        //    ConexionDB conexion = new ConexionDB();
        //    conexion.AbrirConexion();

        //    string query = @"
        //INSERT INTO Avances_Trabajo
        //(NroOrdenTrabajo, Descripcion, Fecha, HorasInsumidas, UserIDAlta, TipoAvance)
        //VALUES
        //(@NroOrdenTrabajo, @Descripcion, @Fecha, @HorasInsumidas, @UserIDAlta, @TipoAvance)";

        //    using (SqlCommand cmd = new SqlCommand(query, conexion.con))
        //    {
        //        cmd.Parameters.AddWithValue("@NroOrdenTrabajo", avance.NroOrdenTrabajo);
        //        cmd.Parameters.AddWithValue("@Descripcion", avance.Descripcion ?? "");
        //        cmd.Parameters.AddWithValue("@Fecha", avance.Fecha);
        //        cmd.Parameters.AddWithValue("@HorasInsumidas", avance.HorasInsumidas);
        //        cmd.Parameters.AddWithValue("@UserIDAlta", avance.UserIDAlta ?? "");
        //        cmd.Parameters.AddWithValue("@TipoAvance", avance.TipoAvance ?? "");

        //        cmd.ExecuteNonQuery();
        //    }
        //}



    }
}

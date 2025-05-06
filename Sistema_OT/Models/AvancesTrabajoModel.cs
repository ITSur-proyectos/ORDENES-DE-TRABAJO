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
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasInsumidas { get; set; }
        public string UserIDAlta { get; set; }



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




    }
}

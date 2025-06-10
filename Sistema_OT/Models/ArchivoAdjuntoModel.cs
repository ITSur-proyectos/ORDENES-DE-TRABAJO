using Sistema_OT.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sistema_OT.Models
{
    public class ArchivoAdjuntoModel
    {
        public int CorrelativoAdjunto { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UserID { get; set; }



        public static List<ArchivoAdjuntoModel> ConseguirAdjuntos(int nroOrden)
        {
            List<ArchivoAdjuntoModel> adjuntos = new List<ArchivoAdjuntoModel>();
            ConexionDB conexionDB = new ConexionDB();
            conexionDB.AbrirConexion();

            string consulta = @"
                SELECT CorrelativoAdjunto, NombreArchivo, FechaAlta, UserID
                FROM Archivos_Adjuntos
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
                            var item = new ArchivoAdjuntoModel
                            {
                                CorrelativoAdjunto = reader.GetInt32(reader.GetOrdinal("CorrelativoAdjunto")),
                                NombreArchivo = reader.GetString(reader.GetOrdinal("NombreArchivo")),
                                FechaAlta = reader.GetDateTime(reader.GetOrdinal("FechaAlta")),
                                UserID = reader.GetString(reader.GetOrdinal("UserID"))
                            };
                            adjuntos.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al obtener adjuntos: " + ex.Message);
                }
            }

            return adjuntos;
        }
    }
}

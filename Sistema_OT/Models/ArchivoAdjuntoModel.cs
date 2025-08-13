using Sistema_OT.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

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

        public static async Task GuardarArchivo(IFormFile file, int nroOrdenTrabajo, string userId)
        {
            if (file == null || file.Length == 0)
                return;

            string carpetaDestino = Path.Combine("wwwroot", "uploads", nroOrdenTrabajo.ToString());

            if (!Directory.Exists(carpetaDestino))
                Directory.CreateDirectory(carpetaDestino);

            try
            {
                ConexionDB conexionDB = new ConexionDB();
                conexionDB.AbrirConexion();

                string fileName = Path.GetFileName(file.FileName);
                string rutaCompleta = Path.Combine(carpetaDestino, fileName);

                // Guardar archivo físicamente
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Obtener siguiente CorrelativoAdjunto
                int correlativo = 1;
                string queryCorrelativo = @"
            SELECT ISNULL(MAX(CorrelativoAdjunto), 0) + 1 
            FROM Archivos_Adjuntos 
            WHERE NroOrdenTrabajo = @NroOrdenTrabajo";

                using (SqlCommand cmdCorrelativo = new SqlCommand(queryCorrelativo, conexionDB.con))
                {
                    cmdCorrelativo.Parameters.AddWithValue("@NroOrdenTrabajo", nroOrdenTrabajo);
                    object result = await cmdCorrelativo.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        correlativo = Convert.ToInt32(result);
                    }
                }

                // Insertar registro en BD usando SP
                using (SqlCommand cmd = new SqlCommand("InsertarArchivoAdjunto", conexionDB.con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NroOrdenTrabajo", nroOrdenTrabajo);
                    cmd.Parameters.AddWithValue("@CorrelativoAdjunto", correlativo);
                    cmd.Parameters.AddWithValue("@NombreArchivo", fileName);
                    cmd.Parameters.AddWithValue("@FechaAlta", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserID", userId);

                    await cmd.ExecuteNonQueryAsync();
                }

                conexionDB.CerrarConexion();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar archivos adjuntos: " + ex.Message);
            }
        }


    }
}

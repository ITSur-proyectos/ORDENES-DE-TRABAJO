using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sistema_OT.Models;
using System;
using AspNetCore;


namespace Sistema_OT.Controllers
{
    public class ArchivosController : Controller
    {
        private readonly string _connectionString = "Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5";

        // Acción para mostrar la vista combinada y manejar la subida
        [HttpGet]
        public async Task<IActionResult> Archivos()
        {
            var archivos = await ObtenerArchivos();
            return View("VistaIndividual", archivos);
        }

        //Metodo privado para obtener la lista de archivos
        private async Task<List<ArchivoViewModel>> ObtenerArchivos()
        {
            var archivos = new List<ArchivoViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, NombreArchivo, FechaSubida FROM Archivos";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            archivos.Add(new ArchivoViewModel
                            {
                                Id = (int)reader["Id"],
                                NombreArchivo = reader["NombreArchivo"].ToString(),
                                FechaSubida = (DateTime)reader["FechaSubida"]
                            });
                        }
                    }
                }
            }

            return archivos;
        }

        
        [HttpPost]
        public IActionResult SubirArchivo(IFormFile archivo, int nroOrdenTrabajo, int correlativoAdjunto, DateTime fechaAlta, string userID)
        {
            if (archivo != null && archivo.Length > 0)
            {
                // Leer el archivo como un arreglo de bytes
                byte[] archivoBytes;
                using (var memoryStream = new MemoryStream())
                {
                    archivo.CopyTo(memoryStream);
                    archivoBytes = memoryStream.ToArray();
                }

                // Insertar en la base de datos
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"INSERT INTO Archivos (NombreArchivo, TipoContenido, Archivo, NroOrdenTrabajo, CorrelativoAdjunto, FechaAlta, UserID) 
                                  VALUES (@NombreArchivo, @TipoContenido, @Archivo, @NroOrdenTrabajo, @CorrelativoAdjunto, @FechaAlta, @UserID)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreArchivo", archivo.FileName);
                        command.Parameters.AddWithValue("@TipoContenido", archivo.ContentType);
                        command.Parameters.AddWithValue("@Archivo", archivoBytes);
                        command.Parameters.AddWithValue("@NroOrdenTrabajo", nroOrdenTrabajo);
                        command.Parameters.AddWithValue("@CorrelativoAdjunto", correlativoAdjunto);
                        command.Parameters.AddWithValue("@FechaAlta", fechaAlta);
                        command.Parameters.AddWithValue("@UserID", userID);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                ViewBag.Message = "Archivo cargado con éxito.";
            }
            else
            {
                ViewBag.Message = "Por favor, selecciona un archivo válido.";
            }

            return RedirectToAction("Archivos");
        }

    }
}

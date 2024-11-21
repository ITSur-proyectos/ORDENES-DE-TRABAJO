using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sistema_OT.Models;
using System;

namespace Sistema_OT.Controllers
{
    public class ArchivosController : Controller
    {
        private readonly string _connectionString = "Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5";

        // Acción para mostrar la vista y cargar la lista de archivos
        [HttpGet]
        public async Task<IActionResult> Archivos()
        {
            var archivos = await ObtenerArchivos();
            return View("~/Views/Home/VistaIndividual.cshtml", archivos);
        }

        // Acción para obtener los archivos (usada por AJAX)
        [HttpGet]
        public async Task<IActionResult> ObtenerArchivos()
        {
            var archivos = await ObtenerArchivosDesdeDB();
            return Json(archivos);  // Devolver los archivos en formato JSON para el frontend
        }

        // Método privado para obtener la lista de archivos desde la base de datos
        private async Task<List<ArchivoViewModel>> ObtenerArchivosDesdeDB()
        {
            var archivos = new List<ArchivoViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, NombreArchivo, FechaSubida FROM Archivos";
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
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

        // Acción para manejar la subida del archivo
        [HttpPost]
        public async Task<IActionResult> SubirArchivo(IFormFile archivo, int nroOrdenTrabajo, int correlativoAdjunto, DateTime fechaAlta, string userID)
        {
            if (archivo == null || archivo.Length == 0)
            {
                TempData["Message"] = "Por favor, selecciona un archivo válido.";
                return RedirectToAction("Archivos");
            }

            // Leer el archivo como un arreglo de bytes
            byte[] archivoBytes;
            using (var memoryStream = new MemoryStream())
            {
                await archivo.CopyToAsync(memoryStream);
                archivoBytes = memoryStream.ToArray();
            }

            // Insertar el archivo en la base de datos
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Archivos 
                              (NombreArchivo, TipoContenido, Archivo, NroOrdenTrabajo, CorrelativoAdjunto, FechaAlta, UserID) 
                              VALUES 
                              (@NombreArchivo, @TipoContenido, @Archivo, @NroOrdenTrabajo, @CorrelativoAdjunto, @FechaAlta, @UserID)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreArchivo", archivo.FileName);
                    command.Parameters.AddWithValue("@TipoContenido", archivo.ContentType);
                    command.Parameters.AddWithValue("@Archivo", archivoBytes);
                    command.Parameters.AddWithValue("@NroOrdenTrabajo", nroOrdenTrabajo);
                    command.Parameters.AddWithValue("@CorrelativoAdjunto", correlativoAdjunto);
                    command.Parameters.AddWithValue("@FechaAlta", fechaAlta);
                    command.Parameters.AddWithValue("@UserID", userID);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }

            // Redirigir con un mensaje de éxito
            TempData["Message"] = "Archivo cargado con éxito.";
            return RedirectToAction("Archivos");
        }

        // Acción para descargar un archivo
        [HttpGet]
        public async Task<IActionResult> DescargarArchivo(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT NombreArchivo, TipoContenido, Archivo FROM Archivos WHERE Id = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            var nombreArchivo = reader["NombreArchivo"].ToString();
                            var tipoContenido = reader["TipoContenido"].ToString();
                            var archivoBytes = (byte[])reader["Archivo"];

                            return File(archivoBytes, tipoContenido, nombreArchivo);
                        }
                    }
                }
            }

            return NotFound("Archivo no encontrado.");
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class FilesController : Controller
{
    //static string cadena = "Data Source=192.168.110.5;Initial Catalog=Db_ITSur_CSharp;User ID=sa;Password=felisa5";

    // Acción para mostrar la vista principal
    public ActionResult VistaIndividual()
    {
        ViewBag.Message = TempData["Message"]?.ToString();
        return View();
    }

    // Acción para cargar los archivos
    [HttpPost]
    [Route("Files/UploadFiles")]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            TempData["Message"] = "No se seleccionaron archivos.";
            return RedirectToAction("VistaIndividual");
        }

        try
        {
            // Ruta principal donde se guardarán los archivos
            var mainUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            // Crear subcarpetas: Documentos, Imágenes, Videos, Otros
            var folders = new Dictionary<string, string>
            {
                { "Documentos", Path.Combine(mainUploadPath, "Documentos") },
                { "Imágenes", Path.Combine(mainUploadPath, "Imagenes") },
                { "Videos", Path.Combine(mainUploadPath, "Videos") },
                { "Otros", Path.Combine(mainUploadPath, "Otros") }
            };

            // Crear las subcarpetas si no existen
            foreach (var folder in folders.Values)
            {
                CreateDirectoryIfNotExists(folder);
            }

            // Procesar cada archivo y guardarlo en la carpeta correspondiente
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Verificar la extensión del archivo y asignarlo a una subcarpeta
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();

                    string uploadPath = fileExtension switch
                    {
                        ".pdf" or ".doc" or ".docx" or ".xls" or ".xlsx" => folders["Documentos"],
                        ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => folders["Imágenes"],
                        ".mp4" or ".avi" or ".mov" or ".mkv" => folders["Videos"],
                        _ => folders["Otros"]
                    };

                    // Evitar colisiones de nombres de archivo
                    string uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{fileExtension}";

                    // Guardar el archivo
                    var filePath = Path.Combine(uploadPath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            TempData["Message"] = "Archivos cargados exitosamente!";
        }
        catch (Exception ex)
        {
            TempData["Message"] = $"Error al cargar los archivos: {ex.Message}";
        }

        return RedirectToAction("VistaIndividual");
    }

    // Método para crear una carpeta si no existe
    private void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}

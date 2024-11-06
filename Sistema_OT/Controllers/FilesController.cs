using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sistema_OT.Controllers
{
    public class FilesController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            // Define la ruta base donde se encuentran las subcarpetas
            var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "files");

            // Asegúrate de que las subcarpetas existen (crea las carpetas si no existen)
            Directory.CreateDirectory(Path.Combine(baseFolder, "imagenes"));
            Directory.CreateDirectory(Path.Combine(baseFolder, "videos"));
            Directory.CreateDirectory(Path.Combine(baseFolder, "documentos"));
            Directory.CreateDirectory(Path.Combine(baseFolder, "otros"));

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    // Obtén la subcarpeta correspondiente para el archivo
                    string subFolder = GetSubFolderForFile(file.FileName);
                    var filePath = Path.Combine(baseFolder, subFolder, file.FileName);

                    // Guarda el archivo en la subcarpeta correspondiente
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    Console.WriteLine($"Archivo guardado en: {filePath}");
                }
            }

            ViewBag.Message = "Archivos subidos exitosamente.";
            return View();
        }

        // Método auxiliar para obtener la subcarpeta en función de la extensión del archivo
        private string GetSubFolderForFile(string fileName)
        {
            // Obtén la extensión del archivo
            string extension = Path.GetExtension(fileName).ToLower();

            // Define qué extensiones corresponden a cada tipo de archivo
            var imageExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var videoExtensions = new List<string> { ".mp4", ".avi", ".mov", ".wmv" };
            var documentExtensions = new List<string> { ".pdf", ".doc", ".docx", ".txt", ".xlsx" };

            // Retorna la subcarpeta correspondiente en función de la extensión
            if (imageExtensions.Contains(extension)) return "imagenes";
            if (videoExtensions.Contains(extension)) return "videos";
            if (documentExtensions.Contains(extension)) return "documentos";

            // Si no coincide con ninguna categoría, retorna la carpeta "otros"
            return "otros";
        }
    }

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class FilesController : Controller
{
    // Acción para cargar los archivos
    [HttpPost]
    [Route("Files/UploadFiles")]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            ModelState.AddModelError("", "No se seleccionaron archivos.");
            return View();
        }

        // Ruta principal donde se guardarán los archivos
        var mainUploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Controllers", "Files");

        
        if (!Directory.Exists(mainUploadPath))
        {
            Directory.CreateDirectory(mainUploadPath);
        }

        // Crear subcarpetas: Documentos, Imagenes, Videos, Otros
        var documentPath = Path.Combine(mainUploadPath, "Documentos");
        var imagesPath = Path.Combine(mainUploadPath, "Imagenes");
        var videosPath = Path.Combine(mainUploadPath, "Videos");
        var othersPath = Path.Combine(mainUploadPath, "Otros");

        // Crear las subcarpetas si no existen
        CreateDirectoryIfNotExists(documentPath);
        CreateDirectoryIfNotExists(imagesPath);
        CreateDirectoryIfNotExists(videosPath);
        CreateDirectoryIfNotExists(othersPath);

        foreach (var file in files)
        {
            // Verificar la extensión del archivo y asignarlo a una subcarpeta
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            string uploadPath = fileExtension switch
            {
                ".pdf" or ".doc" or ".docx" or ".xls" or ".xlsx" => documentPath,  // Documentos
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" => imagesPath,     // Imagenes
                ".mp4" or ".avi" or ".mov" or ".mkv" => videosPath,                // Videos
                _ => othersPath                                                    // Otros
            };

            // Guardar el archivo en la subcarpeta correspondiente
            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        TempData["Message"] = "Archivos cargados exitosamente!";
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

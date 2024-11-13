using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sistema_OT.Controllers
{
    public class FilesController : Controller
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No se han proporcionado archivos para cargar.");
            }

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var allowedExtensions = new Dictionary<string, string>
            {
                { ".jpg", "imagenes" },
                { ".png", "imagenes" },
                { ".pdf", "documentos" },
                { ".docx", "documentos" },
                { ".mp4", "videos" }
                // Añade más extensiones y carpetas según sea necesario
            };

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (allowedExtensions.TryGetValue(extension, out string folderName))
                {
                    var folderPath = Path.Combine(basePath, folderName);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else
                {
                    var otherFolderPath = Path.Combine(basePath, "otros");

                    if (!Directory.Exists(otherFolderPath))
                    {
                        Directory.CreateDirectory(otherFolderPath);
                    }

                    var filePath = Path.Combine(otherFolderPath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return Ok("Archivos cargados exitosamente.");
        }
    }
}

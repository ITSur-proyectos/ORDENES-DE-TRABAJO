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
            var allowedExtensions = new Dictionary<string, string>
            {
                { ".jpg", "imagenes" },
                { ".png", "imagenes" },
                { ".pdf", "documentos" },
                { ".docx", "documentos" },
                { ".txt", "documentos" },  // Agregado .txt para documentos
                { ".mp4", "videos" },
                { ".mp3", "videos" },  // Agregado .mp3 para videos
                { ".gif", "imagenes" },  // Agregado .gif para imágenes
                { ".xlsx", "documentos" }  // Agregado .xlsx para documentos
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
                }
    }
}

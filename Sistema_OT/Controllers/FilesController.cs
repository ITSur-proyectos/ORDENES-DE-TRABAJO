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

            }
    }
}

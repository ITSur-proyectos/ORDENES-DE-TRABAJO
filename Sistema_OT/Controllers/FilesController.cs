using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Sistema_OT.Controllers
{
    public class FilesController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile fileUpload)
        {
            if (fileUpload != null && fileUpload.Length > 0)
            {
                
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileUpload.FileName);

                
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

               
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileUpload.CopyToAsync(stream);
                }

               
                return RedirectToAction("VistaIndividual", "Home");
            }

            
            return View("Error"); 
        }
    }
}

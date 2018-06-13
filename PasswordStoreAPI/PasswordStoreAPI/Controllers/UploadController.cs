using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace PasswordStoreAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            this._hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult get()
        {
            return Ok("Works");
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload(IFormFile value)
        {
            try
            {
                var file = Request.Form.Files[0];
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "uplaod");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok(("Success"));
            }
            catch
            {
                return StatusCode(500, "There was an error uploading your file...");
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrKouk.WebRazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Process([FromForm] IFormFile filePond)
        {
            Debug.WriteLine("");
            var formFile = "";
            var filePath = Path.GetTempFileName();
           

            return Ok();
        }
    }
}
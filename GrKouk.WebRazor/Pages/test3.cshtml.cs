using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GrKouk.WebApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrKouk.WebRazor.Pages
{
    public class test3Model : PageModel
    {
        private readonly ApiDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public test3Model(ApiDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            long uploadedSize = 0;

            string pathForUploadedFiles = _hostingEnvironment.WebRootPath + "\\productImages\\";

            var uploadedFiles = Request.Form.Files;


            int iCounter = 0;
            string sFilesUploaded = "";
            List<string> listFiles = new List<string>();
            foreach (var uploadedFile in uploadedFiles)
            {
                iCounter++;
                uploadedSize += uploadedFile.Length;
                sFilesUploaded += "\n" + uploadedFile.FileName;
                listFiles.Add(uploadedFile.FileName);
                //< Filename >
                var extension = "." + uploadedFile.FileName.Split('.')[uploadedFile.FileName.Split('.').Length - 1];
                string uploadedFilename = uploadedFile.FileName;
                string newFilenameOnServer = pathForUploadedFiles + "\\" + Guid.NewGuid() + extension;
                //</ Filename >
                //< Copy File to Target >

                using (FileStream stream = new FileStream(newFilenameOnServer, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }
            }
            return new JsonResult(listFiles);
        }

        public async Task<IActionResult> OnPostDelete()
        {
            var uploaded_files = Request.Form.Files;
            return new JsonResult("Ok");
        }
    }
}
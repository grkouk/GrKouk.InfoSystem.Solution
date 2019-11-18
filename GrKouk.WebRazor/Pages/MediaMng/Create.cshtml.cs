﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GrKouk.InfoSystem.Domain.MediaEntities;
using Microsoft.AspNetCore.Hosting;

namespace GrKouk.WebRazor.Pages.MediaMng
{
    public class CreateModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CreateModel(GrKouk.WebApi.Data.ApiDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public MediaEntry MediaEntry { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            long uploadedSize = 0;
            string pathForUploadedFiles = _hostingEnvironment.WebRootPath + "\\productimages\\";
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
                string newFileName = Guid.NewGuid() + extension;
                string newFilenameOnServer = pathForUploadedFiles + "\\" + newFileName;

                using (FileStream stream = new FileStream(newFilenameOnServer, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(stream);
                }

                MediaEntry.MediaFile = newFileName;
                MediaEntry.OriginalFileName = uploadedFilename;
                _context.MediaEntries.Add(MediaEntry);
            }
            //return new JsonResult(listFiles);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
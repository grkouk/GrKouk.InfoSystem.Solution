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
    public class test2Model : PageModel
    {
        private readonly ApiDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public test2Model(ApiDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            //< init >

            long uploaded_size = 0;

            string path_for_Uploaded_Files = _hostingEnvironment.WebRootPath + "\\images\\";

            //</ init >



            //< get form_files >

            //IFormFile uploaded_File

            var uploaded_files = Request.Form.Files;

            //</ get form_files >



            //------< @Loop: Uploaded Files >------

            int iCounter = 0;

            string sFiles_uploaded = "";

            List<string> list_Files = new List<string>();

            foreach (var uploaded_file in uploaded_files)

            {

                //----< Uploaded File >----

                iCounter++;

                uploaded_size += uploaded_file.Length;

                sFiles_uploaded += "\n" + uploaded_file.FileName;

                list_Files.Add(uploaded_file.FileName);



                //< Filename >

                string uploaded_Filename = uploaded_file.FileName;

                string new_Filename_on_Server = path_for_Uploaded_Files + "\\" + uploaded_Filename;

                //</ Filename >



                //< Copy File to Target >

                using (FileStream stream = new FileStream(new_Filename_on_Server, FileMode.Create))

                {

                    await uploaded_file.CopyToAsync(stream);

                }

                //< Copy File to Target >

                //----</ Uploaded File >----

            }

            //------</ @Loop: Uploaded Files >------



            return new JsonResult(list_Files);
        }
    }
}
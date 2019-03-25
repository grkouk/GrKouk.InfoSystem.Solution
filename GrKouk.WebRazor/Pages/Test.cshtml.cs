using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GrKouk.WebRazor.Pages
{
    public class TestModel : PageModel
    {
        private readonly IHostingEnvironment _appEnvironment;

        public TestModel(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        [BindProperty]
        public PersonWithPicture itemVm { get; set; }
        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var webRootPath = _appEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count>0)
            {
                var uploads = Path.Combine(webRootPath, @"images");
                var extension = Path.GetExtension(files[0].FileName);
                using (var filestream = new FileStream(Path.Combine(uploads,"10"+extension),FileMode.Create))
                {
                    files[0].CopyTo(filestream);

                }
                //personFromDb.Phote=@"\" + @"images" + @"\" + persons.id + exptension
            }
          

            return RedirectToPage("./Index");
        }
    }

    public class PersonWithPicture
    {
        public string Name { get; set; }
        public string Photo { get; set;}
    }
}
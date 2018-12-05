using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GrKouk.InfoSystem.Domain.Shared;
using GrKouk.WebApi.Data;

namespace GrKouk.WebRazor.Pages.Transactors
{
    public class DeleteModel : PageModel
    {
        private readonly GrKouk.WebApi.Data.ApiDbContext _context;

        public DeleteModel(GrKouk.WebApi.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Transactor Transactor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transactor = await _context.Transactors
                .Include(t => t.TransactorType).FirstOrDefaultAsync(m => m.Id == id);

            if (Transactor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transactor = await _context.Transactors.FindAsync(id);

            if (Transactor != null)
            {
                _context.Transactors.Remove(Transactor);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex.GetBaseException().GetType() == typeof(SqlException))
                    {
                        Int32 ErrorCode = ((SqlException)ex.InnerException).Number;
                        switch (ErrorCode)
                        {
                            case 2627:  // Unique constraint error
                                break;
                            case 547:   // Constraint check violation
                                break;
                            case 2601:  // Duplicated key row error
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        // handle normal exception
                        throw;
                    }
                    
                }
            }

            return RedirectToPage("./Index");
        }

        //private void HandleException(Exception exception)
        //{
        //    if (exception is DbUpdateConcurrencyException concurrencyEx)
        //    {
        //        // A custom exception of yours for concurrency issues
        //        throw new ConcurrencyException();
        //    }
        //    else if (exception is DbUpdateException dbUpdateEx)
        //    {
        //        if (dbUpdateEx.InnerException != null
        //            && dbUpdateEx.InnerException.InnerException != null)
        //        {
        //            if (dbUpdateEx.InnerException.InnerException is SqlException sqlException)
        //            {
        //                switch (sqlException.Number)
        //                {
        //                    case 2627:  // Unique constraint error
        //                    case 547:   // Constraint check violation
        //                    case 2601:  // Duplicated key row error
        //                        // Constraint violation exception
        //                        // A custom exception of yours for concurrency issues
        //                        throw new ConcurrencyException();
        //                    default:
        //                        // A custom exception of yours for other DB issues
        //                        throw new DatabaseAccessException(
        //                            dbUpdateEx.Message, dbUpdateEx.InnerException);
        //                }
        //            }

        //            throw new DatabaseAccessException(dbUpdateEx.Message, dbUpdateEx.InnerException);
        //        }
        //    }
        //}
    }
}

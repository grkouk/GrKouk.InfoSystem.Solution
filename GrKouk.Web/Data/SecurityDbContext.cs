using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GrKouk.Web.Data
{
    public class SecurityDbContext : IdentityDbContext
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {
        }
    }
}

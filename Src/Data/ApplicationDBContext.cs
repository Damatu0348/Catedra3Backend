using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Src.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Src.Data
{
    public class ApplicationDBContext : IdentityDbContext<Usuario>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions) { }
        public DbSet<Post> Posts {get; set;} = null!;
    }
}
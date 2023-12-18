using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Models;

namespace Project_DAW.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Categorie> Categorii { get; set; }
        public DbSet<Imagine> Imagini { get; set; }

        public DbSet<Comentariu> Comentarii { get; set; }
        public DbSet<Intrebare> Intrebari { get; set; }
        public DbSet<Raspuns> Raspunsuri { get; set; }
        public DbSet<SubCategorie> SubCategorii { get; set;}
    }
}
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApp2.Model;

namespace WebApp2.Data1
{
    public class ApplicationDbContext:DbContext 
    {
    
           public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
                {

                }

        public DbSet<Song>Songs { get; set; }

        public DbSet<Artist> Artists{ get; set; }

        public DbSet<Category> Categorys{ get; set; }

       
        

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.CreatedBy = "system"; 
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOn = DateTime.Now;
                    entry.Entity.ModifiedBy = "system"; 
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
    


using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.DataBase
{
    public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
          //  modelBuilder.ApplyConfigurationsFromAssembly(typeof().Assembly);
        }
    }
}
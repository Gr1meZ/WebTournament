using DataAccess.Abstract;
using DataAccess.Common.Enums;
using DataAccess.Common.IdentityModels;
using DataAccess.Domain.Models;
using DataAccess.PostgreSQL.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.PostgreSQL
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Belt> Belts { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Fighter> Fighters { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<WeightCategorie> WeightCategories { get; set; }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<Bracket> Brackets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Fighter>().Property(d => d.Gender).HasConversion(new EnumToStringConverter<Gender>());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FighterEntityConfiguration).Assembly);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);

        }

    }
}
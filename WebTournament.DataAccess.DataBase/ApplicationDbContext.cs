using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using DataAccess.Domain.Models;
using DataAccess.MSSQL.Configuration;
using DataAccess.IdentityModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DataAccess.Common.Enums;


namespace Infrastructure.DataAccess.MSSQL
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
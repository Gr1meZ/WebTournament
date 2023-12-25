using DataAccess.PostgreSQL.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Models;
using WebTournament.Infrastructure.Identity.Models;

namespace WebTournament.Infrastructure.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
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
        public DbSet<BracketWinner> BracketWinners { get; set; }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
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
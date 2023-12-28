using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Infrastructure.Data.Configuration;
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
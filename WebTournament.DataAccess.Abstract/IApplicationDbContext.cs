using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Abstract
{
    public interface IApplicationDbContext
    {
        DbSet<Belt> Belts { get; set; }
        DbSet<Club> Clubs { get; set; }
        DbSet<Fighter> Fighters { get; set; }
        DbSet<Tournament> Tournaments { get; set; }
        DbSet<Trainer> Trainers { get; set; }
        DbSet<WeightCategorie> WeightCategories { get; set; }
        DbSet<AgeGroup> AgeGroups { get; set; }
        DbSet<Bracket> Brackets { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
    

    }

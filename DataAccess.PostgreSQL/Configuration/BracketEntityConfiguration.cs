using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.PostgreSQL.Configuration;

public class BracketEntityConfiguration : IEntityTypeConfiguration<Bracket>
{
    public void Configure(EntityTypeBuilder<Bracket> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.WeightCategorie)
            .WithMany(x => x.Brackets)
            .HasForeignKey(x => x.WeightCategorieId)
            .IsRequired();
        
        builder.HasOne(x => x.Tournament)
            .WithMany(x => x.Brackets)
            .HasForeignKey(x => x.TournamentId)
            .IsRequired();
    }
}
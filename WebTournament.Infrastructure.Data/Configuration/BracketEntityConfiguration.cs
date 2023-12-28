using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebTournament.Domain.Objects.Bracket;

namespace WebTournament.Infrastructure.Data.Configuration;

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
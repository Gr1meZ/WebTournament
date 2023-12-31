using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebTournament.Domain.Objects.BracketWinner;

namespace WebTournament.Infrastructure.Data.Configuration;

public class BracketWinnerEntityConfiguration : IEntityTypeConfiguration<BracketWinner>
{
    public void Configure(EntityTypeBuilder<BracketWinner> builder)
    {
        builder.HasKey(x => x.Id);


        builder.HasOne(x => x.FirstPlacePlayer)
            .WithOne()
            .HasForeignKey<BracketWinner>(x => x.FirstPlaceId);

        builder.HasOne(x => x.SecondPlacePlayer)
            .WithOne()
            .HasForeignKey<BracketWinner>(x => x.SecondPlaceId);

        builder.HasOne(x => x.ThirdPlacePlayer)
            .WithOne()
            .HasForeignKey<BracketWinner>(x => x.ThirdPlaceId);
       
        builder.HasOne(x => x.Bracket)
            .WithOne()
            .HasForeignKey<BracketWinner>(x => x.Id)
            .IsRequired();
        
      
    }
}
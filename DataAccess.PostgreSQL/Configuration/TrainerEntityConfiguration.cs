using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataAccess.PostgreSQL.Configuration
{
    public class TrainerEntityConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Club)
                .WithMany(x => x.Trainers)
                .HasForeignKey(x => x.ClubId)
                .IsRequired();
        }
    }
}

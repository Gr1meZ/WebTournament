using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataBase.Configuration
{
    internal class FighterEntityConfiguration : IEntityTypeConfiguration<Fighter>
    {
        public void Configure(EntityTypeBuilder<Fighter> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Trainer)
                .WithMany(x => x.Fighters)
                .HasForeignKey(x => x.TrainerId)
                .IsRequired();

            builder.HasOne(x => x.WeightCategorie)
                .WithMany(x => x.Fighters)
                .HasForeignKey(x => x.WeightCategorieId)
                .IsRequired();

            builder.HasOne(x => x.Belt)
                .WithMany(x => x.Fighters)
                .HasForeignKey(x => x.BeltId)
                .IsRequired();

            builder.HasOne(x => x.Tournament)
               .WithMany(x => x.Fighters)
               .HasForeignKey(x => x.TournamentId)
               .IsRequired();
        }
    }
}

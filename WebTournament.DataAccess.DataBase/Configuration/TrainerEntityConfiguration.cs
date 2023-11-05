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

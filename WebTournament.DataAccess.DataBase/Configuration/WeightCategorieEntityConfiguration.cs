using DataAccess.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DataAccess.MSSQL.Configuration
{
    public class WeightCategorieEntityConfiguration : IEntityTypeConfiguration<WeightCategorie>
    {
        public void Configure(EntityTypeBuilder<WeightCategorie> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.AgeGroup)
                .WithMany(x => x.WeightCategories)
                .HasForeignKey(x => x.AgeGroupId)
                .IsRequired();
        }
    }
}

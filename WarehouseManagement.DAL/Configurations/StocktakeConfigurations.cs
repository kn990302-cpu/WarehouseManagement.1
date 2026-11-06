using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.DAL.Configurations;

// Người phụ trách: Thái Anh

public class StocktakeConfiguration : IEntityTypeConfiguration<Stocktake>
{
    public void Configure(EntityTypeBuilder<Stocktake> builder)
    {
        builder.ToTable("Stocktakes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Note).HasMaxLength(500);

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StocktakeDetailConfiguration : IEntityTypeConfiguration<StocktakeDetail>
{
    public void Configure(EntityTypeBuilder<StocktakeDetail> builder)
    {
        builder.ToTable("StocktakeDetails");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reason).HasMaxLength(300);

        builder.HasOne(x => x.Stocktake)
            .WithMany(s => s.Details)
            .HasForeignKey(x => x.StocktakeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

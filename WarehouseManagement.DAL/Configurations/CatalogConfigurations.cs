using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.DAL.Configurations;

// Người phụ trách: Thuận

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("Units");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Ràng buộc ở mức database: tồn kho không bao giờ âm (lớp bảo vệ cuối cùng cho phần xuất kho)
        builder.ToTable("Products", t =>
        {
            t.HasCheckConstraint("CK_Products_Quantity_NonNegative", "[Quantity] >= 0");
            t.HasCheckConstraint("CK_Products_MinQuantity_NonNegative", "[MinQuantity] >= 0");
        });
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(30).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.ImportPrice).HasPrecision(18, 2);
        builder.Property(x => x.ExportPrice).HasPrecision(18, 2);

        // Kiểm tra đồng thời (concurrency)
        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Unit)
            .WithMany(u => u.Products)
            .HasForeignKey(x => x.UnitId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

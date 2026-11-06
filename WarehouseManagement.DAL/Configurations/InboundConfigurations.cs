using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.DAL.Configurations;

// Người phụ trách: Đức

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(300);
    }
}

public class ImportReceiptConfiguration : IEntityTypeConfiguration<ImportReceipt>
{
    public void Configure(EntityTypeBuilder<ImportReceipt> builder)
    {
        builder.ToTable("ImportReceipts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(500);

        builder.HasOne(x => x.Supplier)
            .WithMany(s => s.ImportReceipts)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ImportReceiptDetailConfiguration : IEntityTypeConfiguration<ImportReceiptDetail>
{
    public void Configure(EntityTypeBuilder<ImportReceiptDetail> builder)
    {
        builder.ToTable("ImportReceiptDetails", t =>
            t.HasCheckConstraint("CK_ImportReceiptDetails_Quantity_Positive", "[Quantity] > 0"));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);

        // Xóa phiếu thì xóa luôn chi tiết
        builder.HasOne(x => x.ImportReceipt)
            .WithMany(r => r.Details)
            .HasForeignKey(x => x.ImportReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        // Không cho xóa hàng hóa đã có trong phiếu nhập
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

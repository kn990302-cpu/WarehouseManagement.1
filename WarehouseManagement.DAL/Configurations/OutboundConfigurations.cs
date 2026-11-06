using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.DAL.Configurations;

// Người phụ trách: Thành

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.Address).HasMaxLength(300);
    }
}

public class ExportReceiptConfiguration : IEntityTypeConfiguration<ExportReceipt>
{
    public void Configure(EntityTypeBuilder<ExportReceipt> builder)
    {
        builder.ToTable("ExportReceipts");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.Note).HasMaxLength(500);

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.ExportReceipts)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ExportReceiptDetailConfiguration : IEntityTypeConfiguration<ExportReceiptDetail>
{
    public void Configure(EntityTypeBuilder<ExportReceiptDetail> builder)
    {
        builder.ToTable("ExportReceiptDetails", t =>
            t.HasCheckConstraint("CK_ExportReceiptDetails_Quantity_Positive", "[Quantity] > 0"));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UnitPrice).HasPrecision(18, 2);

        builder.HasOne(x => x.ExportReceipt)
            .WithMany(r => r.Details)
            .HasForeignKey(x => x.ExportReceiptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

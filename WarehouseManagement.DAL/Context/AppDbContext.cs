using Microsoft.EntityFrameworkCore;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.DAL.Context;

/// <summary>
/// DbContext duy nhất của dự án.
/// TẤT CẢ DbSet đã được khai báo sẵn ở đây từ tuần 1 để mọi người KHÔNG phải sửa file này
/// (đây là file dễ conflict nhất). Cấu hình từng bảng nằm trong thư mục Configurations/.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // ===== Khang: Hệ thống =====
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();

    // ===== Thuận: Danh mục hàng hóa =====
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Product> Products => Set<Product>();

    // ===== Đức: Nhập kho =====
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<ImportReceipt> ImportReceipts => Set<ImportReceipt>();
    public DbSet<ImportReceiptDetail> ImportReceiptDetails => Set<ImportReceiptDetail>();

    // ===== Thành: Xuất kho =====
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ExportReceipt> ExportReceipts => Set<ExportReceipt>();
    public DbSet<ExportReceiptDetail> ExportReceiptDetails => Set<ExportReceiptDetail>();

    // ===== Thái Anh: Kiểm kê =====
    public DbSet<Stocktake> Stocktakes => Set<Stocktake>();
    public DbSet<StocktakeDetail> StocktakeDetails => Set<StocktakeDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tự động nạp mọi class IEntityTypeConfiguration<T> trong thư mục Configurations/
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

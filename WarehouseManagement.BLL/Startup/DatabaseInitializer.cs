using Microsoft.EntityFrameworkCore;
using WarehouseManagement.BLL.Auth;
using WarehouseManagement.DAL.Context;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.BLL.Startup;

/// <summary>
/// Chạy khi mở ứng dụng:
/// 1. Đọc chuỗi kết nối và cấu hình DbContextProvider.
/// 2. Tự áp dụng migration mới nhất (thành viên pull code về chỉ cần bấm Run, không phải chạy Update-Database).
/// 3. Tạo tài khoản admin mặc định nếu database chưa có người dùng nào.
/// </summary>
public static class DatabaseInitializer
{
    public const string DefaultAdminUsername = "admin";
    public const string DefaultAdminPassword = "admin123";

    public static void Initialize(string settingsFolder)
    {
        DbContextProvider.Configure(ConnectionStringReader.Read(settingsFolder));

        using var db = DbContextProvider.Create();

        if (!db.Database.GetMigrations().Any())
        {
            throw new InvalidOperationException(
                "Chưa có migration nào.\n\n" +
                "Mở Tools > NuGet Package Manager > Package Manager Console, chọn Default project là WarehouseManagement.DAL rồi chạy:\n" +
                "Add-Migration InitialCreate\n\n" +
                "Sau đó chạy lại ứng dụng.");
        }

        db.Database.Migrate();
        SeedDefaultAdmin(db);
    }

    private static void SeedDefaultAdmin(AppDbContext db)
    {
        if (db.Users.Any())
        {
            return;
        }

        db.Users.Add(new User
        {
            Username = DefaultAdminUsername,
            PasswordHash = PasswordHasher.Hash(DefaultAdminPassword),
            FullName = "Quản trị viên",
            RoleId = RoleIds.Admin,
            IsActive = true,
        });

        db.SaveChanges();
    }
}

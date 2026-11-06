using Microsoft.EntityFrameworkCore;

namespace WarehouseManagement.DAL.Context;

/// <summary>
/// Nơi tạo AppDbContext khi chạy ứng dụng.
/// Cách dùng trong BLL:
/// <code>
/// using var db = DbContextProvider.Create();
/// var products = db.Products.ToList();
/// </code>
/// Mỗi thao tác nên tạo một DbContext mới rồi dispose (dùng "using"), không giữ DbContext lâu.
/// </summary>
public static class DbContextProvider
{
    private static DbContextOptions<AppDbContext>? _options;

    /// <summary>Gọi một lần duy nhất khi ứng dụng khởi động (đã làm sẵn trong App.xaml.cs).</summary>
    public static void Configure(string connectionString)
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;
    }

    public static AppDbContext Create()
    {
        if (_options is null)
        {
            throw new InvalidOperationException(
                "DbContextProvider chưa được cấu hình. Hãy gọi DbContextProvider.Configure(...) khi khởi động.");
        }

        return new AppDbContext(_options);
    }
}

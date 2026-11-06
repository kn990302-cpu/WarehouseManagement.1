using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WarehouseManagement.DAL.Context;

/// <summary>
/// Chỉ dùng cho công cụ EF Core (Add-Migration, Update-Database) trong Package Manager Console.
/// Ứng dụng khi chạy thật KHÔNG dùng class này.
/// </summary>
public class AppDbContextDesignFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = ConnectionStringReader.Read(FindSettingsFolder());

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AppDbContext(options);
    }

    /// <summary>Tìm thư mục chứa appsettings.json (thường là project UI).</summary>
    private static string FindSettingsFolder()
    {
        var current = Directory.GetCurrentDirectory();
        string[] candidates =
        [
            current,
            Path.Combine(current, "WarehouseManagement.UI"),
            Path.Combine(current, "..", "WarehouseManagement.UI"),
        ];

        foreach (var folder in candidates)
        {
            if (File.Exists(Path.Combine(folder, "appsettings.json")))
            {
                return Path.GetFullPath(folder);
            }
        }

        return current;
    }
}

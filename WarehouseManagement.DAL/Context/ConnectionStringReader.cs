using Microsoft.Extensions.Configuration;

namespace WarehouseManagement.DAL.Context;

/// <summary>
/// Đọc chuỗi kết nối từ appsettings.json (và appsettings.Local.json nếu có).
/// appsettings.Local.json KHÔNG được đẩy lên Git, dùng khi máy bạn cần chuỗi kết nối khác
/// (ví dụ dùng SQL Server Express thay vì LocalDB).
/// </summary>
public static class ConnectionStringReader
{
    public const string ConnectionName = "WarehouseDb";

    /// <summary>Dự phòng khi không tìm thấy appsettings.json.</summary>
    public const string DefaultConnectionString =
        @"Server=(localdb)\MSSQLLocalDB;Database=WarehouseManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

    public static string Read(string basePath)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Local.json", optional: true)
            .Build();

        var value = configuration.GetConnectionString(ConnectionName);
        return string.IsNullOrWhiteSpace(value) ? DefaultConnectionString : value;
    }
}

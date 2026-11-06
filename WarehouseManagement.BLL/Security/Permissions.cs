using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.BLL.Security;

/// <summary>Các chức năng (menu) trong ứng dụng.</summary>
public enum AppModule
{
    Dashboard,
    Products,
    Suppliers,
    Import,
    Customers,
    Export,
    Stocktake,
    Reports,
    Users,
}

/// <summary>
/// Bảng phân quyền: vai trò nào được dùng chức năng nào.
/// Muốn đổi quyền chỉ cần sửa bảng dưới đây, menu sẽ tự ẩn/hiện theo.
/// </summary>
public static class Permissions
{
    private static readonly Dictionary<string, HashSet<AppModule>> Map = new()
    {
        // Quản trị viên
        [RoleCodes.Admin] = [.. Enum.GetValues<AppModule>()],

        // Quản lý: toàn bộ nghiệp vụ kho và báo cáo, không quản lý tài khoản
        [RoleCodes.Manager] =
        [
            AppModule.Dashboard, AppModule.Products, AppModule.Suppliers, AppModule.Import,
            AppModule.Customers, AppModule.Export, AppModule.Stocktake, AppModule.Reports,
        ],

        // Thủ kho: thao tác nhập/xuất/kiểm kê hằng ngày, không xem báo cáo
        [RoleCodes.WarehouseKeeper] =
        [
            AppModule.Dashboard, AppModule.Products, AppModule.Suppliers, AppModule.Import,
            AppModule.Customers, AppModule.Export, AppModule.Stocktake,
        ],
    };

    public static bool CanAccess(string? roleCode, AppModule module) =>
        roleCode is not null
        && Map.TryGetValue(roleCode, out var modules)
        && modules.Contains(module);
}

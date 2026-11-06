using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using WarehouseManagement.BLL.Security;
using WarehouseManagement.UI.Views.Admin;
using WarehouseManagement.UI.Views.Catalog;
using WarehouseManagement.UI.Views.Inbound;
using WarehouseManagement.UI.Views.Outbound;
using WarehouseManagement.UI.Views.Reports;

namespace WarehouseManagement.UI.Navigation;

/// <summary>Một mục trên menu bên trái.</summary>
public sealed record NavItem(string Title, PackIconKind Icon, AppModule Module, Func<UserControl> CreateView);

/// <summary>
/// Danh sách menu của MainWindow. ĐÃ ĐĂNG KÝ SẴN màn hình cho cả 5 người,
/// nên mọi người KHÔNG cần sửa file này hay MainWindow. Chỉ cần làm việc trong file View của mình.
/// </summary>
public static class NavigationRegistry
{
    public static IReadOnlyList<NavItem> All { get; } =
    [
        // Thái Anh
        new("Tổng quan", PackIconKind.ViewDashboard, AppModule.Dashboard, () => new DashboardView()),

        // Thuận
        new("Hàng hóa", PackIconKind.PackageVariantClosed, AppModule.Products, () => new ProductView()),

        // Đức
        new("Nhà cung cấp", PackIconKind.TruckDelivery, AppModule.Suppliers, () => new SupplierView()),
        new("Nhập kho", PackIconKind.TrayArrowDown, AppModule.Import, () => new ImportReceiptView()),

        // Thành
        new("Khách hàng", PackIconKind.AccountGroup, AppModule.Customers, () => new CustomerView()),
        new("Xuất kho", PackIconKind.TrayArrowUp, AppModule.Export, () => new ExportReceiptView()),

        // Thái Anh
        new("Kiểm kê", PackIconKind.ClipboardCheckOutline, AppModule.Stocktake, () => new StocktakeView()),
        new("Báo cáo", PackIconKind.FileChartOutline, AppModule.Reports, () => new ReportView()),

        // Khang
        new("Người dùng", PackIconKind.AccountKey, AppModule.Users, () => new UserManagementView()),
    ];
}

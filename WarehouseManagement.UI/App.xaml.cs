using System.Windows;
using System.Windows.Threading;
using WarehouseManagement.BLL.Startup;
using WarehouseManagement.UI.Views.Auth;

namespace WarehouseManagement.UI;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Bắt mọi lỗi chưa xử lý để app không bị tắt đột ngột (hữu ích khi "phá app" ở tuần 4)
        DispatcherUnhandledException += OnUnhandledException;

        try
        {
            DatabaseInitializer.Initialize(AppContext.BaseDirectory);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Không khởi tạo được cơ sở dữ liệu.\n\n" + GetInnermostMessage(ex) +
                "\n\nGợi ý:\n" +
                "• Đã cài SQL Server Express LocalDB chưa? (chạy 'sqllocaldb info' để kiểm tra)\n" +
                "• Nếu vừa sửa Entity mà chưa tạo migration: chạy Add-Migration <TenMoTa>.",
                "Lỗi khởi động",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Shutdown(1);
            return;
        }

        var login = new LoginWindow();
        base.MainWindow = login;
        login.Show();
    }

    private static void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            "Đã xảy ra lỗi:\n\n" + GetInnermostMessage(e.Exception),
            "Lỗi",
            MessageBoxButton.OK,
            MessageBoxImage.Error);

        e.Handled = true;
    }

    private static string GetInnermostMessage(Exception ex)
    {
        while (ex.InnerException is not null)
        {
            ex = ex.InnerException;
        }

        return ex.Message;
    }
}

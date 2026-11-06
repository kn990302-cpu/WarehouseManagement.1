using System.Windows;
using WarehouseManagement.BLL.Auth;

namespace WarehouseManagement.UI.Views.Auth;

// Người phụ trách: Nguyễn Bình Khang
/// <summary>Hộp thoại cho người đang đăng nhập tự đổi mật khẩu của mình.</summary>
public partial class ChangePasswordWindow : Window
{
    private readonly AuthService _authService = new();

    public ChangePasswordWindow()
    {
        InitializeComponent();

        var user = UserSession.RequireUser();
        AccountText.Text = $"Tài khoản: {user.Username} ({user.FullName})";
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var result = _authService.ChangePassword(
            CurrentPasswordBox.Password,
            NewPasswordBox.Password,
            ConfirmPasswordBox.Password);

        if (!result.Success)
        {
            ErrorText.Text = result.Message;
            return;
        }

        MessageBox.Show(
            this,
            "Đổi mật khẩu thành công. Lần đăng nhập sau hãy dùng mật khẩu mới.",
            "Đổi mật khẩu",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        DialogResult = true; // Đóng hộp thoại
    }
}

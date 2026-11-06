using System.Windows;
using WarehouseManagement.BLL.Auth;

namespace WarehouseManagement.UI.Views.Auth;

public partial class LoginWindow : Window
{
    private readonly AuthService _authService = new();

    public LoginWindow()
    {
        InitializeComponent();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var username = UsernameBox.Text;
        var password = PasswordBox.Password;

        SetBusy(true);
        try
        {
            // Chạy ở luồng nền để giao diện không bị đơ khi chờ database
            var result = await Task.Run(() => _authService.Login(username, password));

            if (!result.Success)
            {
                ErrorText.Text = result.Message;
                PasswordBox.Clear();
                PasswordBox.Focus();
                return;
            }

            var main = new MainWindow();
            Application.Current.MainWindow = main;
            main.Show();
            Close();
        }
        catch (Exception ex)
        {
            ErrorText.Text = "Không kết nối được cơ sở dữ liệu: " + ex.Message;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy)
    {
        LoginButton.IsEnabled = !busy;
        UsernameBox.IsEnabled = !busy;
        PasswordBox.IsEnabled = !busy;
        LoadingBar.Visibility = busy ? Visibility.Visible : Visibility.Collapsed;

        if (busy)
        {
            ErrorText.Text = string.Empty;
        }
    }
}

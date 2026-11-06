using System.Windows;
using System.Windows.Controls;
using WarehouseManagement.BLL.Auth;
using WarehouseManagement.BLL.Security;
using WarehouseManagement.UI.Navigation;
using WarehouseManagement.UI.Views.Auth;

namespace WarehouseManagement.UI;

public partial class MainWindow : Window
{
    private readonly AuthService _authService = new();

    public MainWindow()
    {
        InitializeComponent();

        var user = UserSession.RequireUser();
        UserNameText.Text = user.FullName;
        UserRoleText.Text = user.RoleName;

        //  menu mà vai trò hiện tại được phép dùng
        NavList.ItemsSource = NavigationRegistry.All
            .Where(item => Permissions.CanAccess(user.RoleCode, item.Module))
            .ToList();

        NavList.SelectedIndex = 0;
    }

    private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (NavList.SelectedItem is NavItem item)
        {
            ContentArea.Content = item.CreateView();
        }
    }

    private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ChangePasswordWindow { Owner = this };
        dialog.ShowDialog();
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        var confirm = MessageBox.Show(
            "Bạn có chắc muốn đăng xuất?",
            "Đăng xuất",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes)
        {
            return;
        }

        _authService.Logout();

        var login = new LoginWindow();
        Application.Current.MainWindow = login;
        login.Show();
        Close();
    }
}

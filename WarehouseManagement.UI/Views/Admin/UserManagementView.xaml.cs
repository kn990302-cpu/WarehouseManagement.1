using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WarehouseManagement.BLL.Common;
using WarehouseManagement.BLL.Services;

namespace WarehouseManagement.UI.Views.Admin;

// Người phụ trách: Nguyễn Bình Khang
public partial class UserManagementView : UserControl
{
    private readonly UserService _userService = new();

    /// <summary>Tài khoản đang được chọn để sửa. null = đang thêm mới.</summary>
    private UserListItem? _selected;

    public UserManagementView()
    {
        InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        RoleBox.ItemsSource = _userService.GetRoles();
        LoadUsers();
        ResetForm();
    }

    // ===== Danh sách =====

    private void LoadUsers(int? selectId = null)
    {
        var users = _userService.GetAll(SearchBox.Text);
        UserGrid.ItemsSource = users;

        if (selectId is not null)
        {
            UserGrid.SelectedItem = users.FirstOrDefault(u => u.Id == selectId);
        }
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e) => LoadUsers();

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            LoadUsers();
        }
    }

    private void UserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UserGrid.SelectedItem is UserListItem user)
        {
            FillForm(user);
        }
    }

    // ===== Form =====

    private void ResetForm()
    {
        _selected = null;
        UserGrid.SelectedItem = null;

        FormTitle.Text = "Thêm tài khoản mới";
        UsernameBox.Text = string.Empty;
        UsernameBox.IsEnabled = true;
        FullNameBox.Text = string.Empty;
        RoleBox.SelectedIndex = -1;
        PasswordInput.Clear();

        ToggleActiveButton.IsEnabled = false;
        ToggleActiveButton.Content = "KHÓA";
        ResetPasswordButton.IsEnabled = false;
    }

    private void FillForm(UserListItem user)
    {
        _selected = user;

        FormTitle.Text = $"Sửa tài khoản: {user.Username}";
        UsernameBox.Text = user.Username;
        UsernameBox.IsEnabled = false; // Không cho đổi tên đăng nhập
        FullNameBox.Text = user.FullName;
        RoleBox.SelectedValue = user.RoleId;
        PasswordInput.Clear();

        ToggleActiveButton.IsEnabled = true;
        ToggleActiveButton.Content = user.IsActive ? "KHÓA" : "MỞ KHÓA";
        ResetPasswordButton.IsEnabled = true;
    }

    private void NewButton_Click(object sender, RoutedEventArgs e)
    {
        ResetForm();
        MessageText.Text = string.Empty;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (RoleBox.SelectedValue is not int roleId)
        {
            ShowMessage(ServiceResult.Fail("Vui lòng chọn vai trò."));
            return;
        }

        var editingId = _selected?.Id;

        var result = editingId is null
            ? _userService.Create(UsernameBox.Text, FullNameBox.Text, PasswordInput.Password, roleId)
            : _userService.Update(editingId.Value, FullNameBox.Text, roleId);

        ShowMessage(result);
        if (!result.Success)
        {
            return;
        }

        if (editingId is null)
        {
            LoadUsers();
            ResetForm();
        }
        else
        {
            LoadUsers(editingId);
        }
    }

    private void ToggleActiveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selected is null)
        {
            return;
        }

        var action = _selected.IsActive ? "khóa" : "mở khóa";
        var confirm = MessageBox.Show(
            $"Bạn có chắc muốn {action} tài khoản '{_selected.Username}'?",
            "Xác nhận",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes)
        {
            return;
        }

        var id = _selected.Id;
        var result = _userService.ToggleActive(id);
        ShowMessage(result);

        if (result.Success)
        {
            LoadUsers(id);
        }
    }

    private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selected is null)
        {
            return;
        }

        var result = _userService.ResetPassword(_selected.Id, PasswordInput.Password);
        ShowMessage(result);

        if (result.Success)
        {
            PasswordInput.Clear();
        }
    }

    private void ShowMessage(ServiceResult result)
    {
        MessageText.Text = result.Message;
        MessageText.Foreground = result.Success ? Brushes.SeaGreen : Brushes.Firebrick;
    }
}

using Microsoft.EntityFrameworkCore;
using WarehouseManagement.DAL.Context;

namespace WarehouseManagement.BLL.Auth;

public sealed record LoginResult(bool Success, string Message)
{
    public static LoginResult Ok() => new(true, string.Empty);
    public static LoginResult Fail(string message) => new(false, message);
}

/// <summary>Đăng nhập, đăng xuất, đổi mật khẩu.</summary>
public class AuthService
{
    private const string WrongCredentials = "Tên đăng nhập hoặc mật khẩu không đúng.";
    
    public LoginResult Login(string username, string password)
    {
        username = (username ?? string.Empty).Trim();

        if (username.Length == 0)
        {
            return LoginResult.Fail("Vui lòng nhập tên đăng nhập.");
        }

        if (string.IsNullOrEmpty(password))
        {
            return LoginResult.Fail("Vui lòng nhập mật khẩu.");
        }

        using var db = DbContextProvider.Create();

        var user = db.Users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Username == username);

        // Không nói rõ sai tên hay sai mật khẩu, tránh lộ thông tin tài khoản
        if (user is null || !PasswordHasher.Verify(password, user.PasswordHash))
        {
            return LoginResult.Fail(WrongCredentials);
        }

        if (!user.IsActive)
        {
            return LoginResult.Fail("Tài khoản đã bị khóa. Liên hệ quản trị viên.");
        }

        user.LastLoginAt = DateTime.Now;
        db.SaveChanges();

        UserSession.Start(new SessionUser(user.Id, user.Username, user.FullName, user.Role.Code, user.Role.Name));
        return LoginResult.Ok();
    }

    public void Logout() => UserSession.End();

    public LoginResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var sessionUser = UserSession.RequireUser();

        if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6)
        {
            return LoginResult.Fail("Mật khẩu mới phải có ít nhất 6 ký tự.");
        }

        if (newPassword != confirmPassword)
        {
            return LoginResult.Fail("Xác nhận mật khẩu không khớp.");
        }

        if (newPassword == currentPassword)
        {
            return LoginResult.Fail("Mật khẩu mới phải khác mật khẩu hiện tại.");
        }

        using var db = DbContextProvider.Create();
        var user = db.Users.FirstOrDefault(u => u.Id == sessionUser.Id);

        if (user is null || !PasswordHasher.Verify(currentPassword, user.PasswordHash))
        {
            return LoginResult.Fail("Mật khẩu hiện tại không đúng.");
        }

        user.PasswordHash = PasswordHasher.Hash(newPassword);
        db.SaveChanges();
        return LoginResult.Ok();
    }
}

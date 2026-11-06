using Microsoft.EntityFrameworkCore;
using WarehouseManagement.BLL.Auth;
using WarehouseManagement.BLL.Common;
using WarehouseManagement.BLL.Security;
using WarehouseManagement.DAL.Context;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.BLL.Services;

// Người phụ trách: Nguyễn Bình Khang

/// <summary>Một dòng trong bảng danh sách tài khoản (không chứa mật khẩu).</summary>
public sealed record UserListItem(
    int Id, string Username, string FullName, int RoleId, string RoleName,
    bool IsActive, DateTime CreatedAt, DateTime? LastLoginAt)
{
    public string StatusText => IsActive ? "Hoạt động" : "Đã khóa";
}

/// <summary>Một lựa chọn trong ô chọn vai trò.</summary>
public sealed record RoleOption(int Id, string Name);

/// <summary>Quản lý tài khoản quyền AppModule.Users (Quản trị viên) mới gọi được.</summary>
public class UserService
{
    private const int MinPasswordLength = 6;

    public List<UserListItem> GetAll(string? keyword = null)
    {
        RequireAdmin();
        using var db = DbContextProvider.Create();

        var query = db.Users.AsNoTracking().Include(u => u.Role).AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var k = keyword.Trim();
            query = query.Where(u => u.Username.Contains(k) || u.FullName.Contains(k));
        }

        return query
            .OrderBy(u => u.Username)
            .Select(u => new UserListItem(
                u.Id, u.Username, u.FullName, u.RoleId, u.Role.Name,
                u.IsActive, u.CreatedAt, u.LastLoginAt))
            .ToList();
    }

    public List<RoleOption> GetRoles()
    {
        using var db = DbContextProvider.Create();
        return db.Roles.AsNoTracking()
            .OrderBy(r => r.Id)
            .Select(r => new RoleOption(r.Id, r.Name))
            .ToList();
    }

    public ServiceResult Create(string username, string fullName, string password, int roleId)
    {
        RequireAdmin();
        username = (username ?? string.Empty).Trim();
        fullName = (fullName ?? string.Empty).Trim();

        if (username.Length < 3 || username.Length > 50)
            return ServiceResult.Fail("Tên đăng nhập phải từ 3 đến 50 ký tự.");
        if (username.Any(char.IsWhiteSpace))
            return ServiceResult.Fail("Tên đăng nhập không được chứa khoảng trắng.");
        if (ValidateFullName(fullName) is { } nameError)
            return ServiceResult.Fail(nameError);
        if (ValidatePassword(password) is { } passwordError)
            return ServiceResult.Fail(passwordError);

        using var db = DbContextProvider.Create();

        if (!db.Roles.Any(r => r.Id == roleId))
            return ServiceResult.Fail("Vai trò không hợp lệ.");
        if (db.Users.Any(u => u.Username == username))
            return ServiceResult.Fail($"Tên đăng nhập '{username}' đã tồn tại.");

        db.Users.Add(new User
        {
            Username = username,
            FullName = fullName,
            PasswordHash = PasswordHasher.Hash(password),
            RoleId = roleId,
            IsActive = true,
        });
        db.SaveChanges();

        return ServiceResult.Ok($"Đã tạo tài khoản '{username}'.");
    }

    public ServiceResult Update(int userId, string fullName, int roleId)
    {
        var current = RequireAdmin();
        fullName = (fullName ?? string.Empty).Trim();

        if (ValidateFullName(fullName) is { } nameError)
            return ServiceResult.Fail(nameError);

        using var db = DbContextProvider.Create();
        var user = db.Users.FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return ServiceResult.Fail("Không tìm thấy tài khoản.");
        if (!db.Roles.Any(r => r.Id == roleId))
            return ServiceResult.Fail("Vai trò không hợp lệ.");

        if (user.RoleId != roleId)
        {
            if (user.Id == current.Id)
                return ServiceResult.Fail("Bạn không thể tự đổi vai trò của chính mình.");
            if (IsLastActiveAdmin(db, user))
                return ServiceResult.Fail("Không thể đổi vai trò của Quản trị viên cuối cùng.");
        }

        user.FullName = fullName;
        user.RoleId = roleId;
        db.SaveChanges();

        return ServiceResult.Ok("Đã lưu thay đổi.");
    }

    public ServiceResult ToggleActive(int userId)
    {
        var current = RequireAdmin();
        using var db = DbContextProvider.Create();
        var user = db.Users.FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return ServiceResult.Fail("Không tìm thấy tài khoản.");

        if (user.IsActive)
        {
            if (user.Id == current.Id)
                return ServiceResult.Fail("Bạn không thể tự khóa tài khoản đang đăng nhập.");
            if (IsLastActiveAdmin(db, user))
                return ServiceResult.Fail("Không thể khóa Quản trị viên cuối cùng.");
        }

        user.IsActive = !user.IsActive;
        db.SaveChanges();

        return ServiceResult.Ok(user.IsActive
            ? $"Đã mở khóa tài khoản '{user.Username}'."
            : $"Đã khóa tài khoản '{user.Username}'.");
    }

    public ServiceResult ResetPassword(int userId, string newPassword)
    {
        RequireAdmin();

        if (ValidatePassword(newPassword) is { } passwordError)
            return ServiceResult.Fail(passwordError);

        using var db = DbContextProvider.Create();
        var user = db.Users.FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return ServiceResult.Fail("Không tìm thấy tài khoản.");

        user.PasswordHash = PasswordHasher.Hash(newPassword);
        db.SaveChanges();

        return ServiceResult.Ok($"Đã đặt lại mật khẩu cho '{user.Username}'.");
    }

    // ===== Hàm phụ =====

    /// <summary>Chặn ở tầng nghiệp vụ: dù giao diện có lỗi thì người không có quyền vẫn không gọi được.</summary>
    private static SessionUser RequireAdmin()
    {
        var current = UserSession.RequireUser();
        if (!Permissions.CanAccess(current.RoleCode, AppModule.Users))
        {
            throw new UnauthorizedAccessException("Bạn không có quyền quản lý tài khoản.");
        }

        return current;
    }

    /// <summary>Luôn phải còn ít nhất 1 Quản trị viên đang hoạt động, nếu không sẽ không ai quản lý được tài khoản.</summary>
    private static bool IsLastActiveAdmin(AppDbContext db, User user) =>
        user.RoleId == RoleIds.Admin
        && user.IsActive
        && db.Users.Count(u => u.RoleId == RoleIds.Admin && u.IsActive) == 1;

    private static string? ValidateFullName(string fullName) =>
        fullName.Length == 0 ? "Vui lòng nhập họ tên."
        : fullName.Length > 100 ? "Họ tên tối đa 100 ký tự."
        : null;

    private static string? ValidatePassword(string? password) =>
        string.IsNullOrEmpty(password) || password.Length < MinPasswordLength
            ? $"Mật khẩu phải có ít nhất {MinPasswordLength} ký tự."
            : null;
}

namespace WarehouseManagement.BLL.Auth;

/// <summary>Thông tin người đang đăng nhập (không chứa mật khẩu).</summary>
public sealed record SessionUser(int Id, string Username, string FullName, string RoleCode, string RoleName);

/// <summary>
/// Lưu người dùng đang đăng nhập. Các module dùng để:
/// - Lấy Id người lập phiếu: <c>UserSession.Current!.Id</c>
/// - Kiểm tra quyền: <c>Permissions.CanAccess(UserSession.Current?.RoleCode, AppModule.Export)</c>
/// </summary>
public static class UserSession
{
    public static SessionUser? Current { get; private set; }

    public static bool IsLoggedIn => Current is not null;

    internal static void Start(SessionUser user) => Current = user;

    public static void End() => Current = null;

    /// <summary>Lấy người dùng hiện tại, ném lỗi nếu chưa đăng nhập.</summary>
    public static SessionUser RequireUser() =>
        Current ?? throw new InvalidOperationException("Chưa đăng nhập.");
}

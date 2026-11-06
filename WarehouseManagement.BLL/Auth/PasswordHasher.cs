namespace WarehouseManagement.BLL.Auth;

/// <summary>Bọc thư viện BCrypt để cả nhóm dùng chung một cách băm mật khẩu.</summary>
public static class PasswordHasher
{
    private const int WorkFactor = 11;

    public static string Hash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);

    public static bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrEmpty(passwordHash))
        {
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Hash trong DB bị hỏng hoặc không phải BCrypt
            return false;
        }
    }
}

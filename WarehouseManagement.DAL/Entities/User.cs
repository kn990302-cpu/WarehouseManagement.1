namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Khang
public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    /// <summary>Mật khẩu đã băm bằng BCrypt. KHÔNG BAO GIỜ lưu mật khẩu dạng thô.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? LastLoginAt { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}

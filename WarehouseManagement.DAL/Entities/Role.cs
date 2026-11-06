namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Khang
public class Role
{
    public int Id { get; set; }

    /// <summary>Mã vai trò, dùng trong code để phân quyền. Xem <see cref="RoleCodes"/>.</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Tên hiển thị (tiếng Việt).</summary>
    public string Name { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}

/// <summary>Các mã vai trò cố định. Dùng hằng số này thay vì gõ chuỗi tay.</summary>
public static class RoleCodes
{
    public const string Admin = "ADMIN";
    public const string Manager = "MANAGER";
    public const string WarehouseKeeper = "KEEPER";
}

/// <summary>Id cố định của các vai trò được seed sẵn.</summary>
public static class RoleIds
{
    public const int Admin = 1;
    public const int Manager = 2;
    public const int WarehouseKeeper = 3;
}

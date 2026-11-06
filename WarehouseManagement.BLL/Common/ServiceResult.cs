namespace WarehouseManagement.BLL.Common;

/// <summary>
/// Kết quả chung cho các hàm nghiệp vụ: thành công hay không, kèm thông báo cho người dùng.
/// Cả nhóm có thể dùng lại class này trong Service của mình.
/// </summary>
public sealed record ServiceResult(bool Success, string Message)
{
    public static ServiceResult Ok(string message = "") => new(true, message);
    public static ServiceResult Fail(string message) => new(false, message);
}

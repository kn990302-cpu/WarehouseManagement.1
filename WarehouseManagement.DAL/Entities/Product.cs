namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Thuận (bản nháp, Thuận bổ sung thêm trường nếu cần)
public class Product
{
    public int Id { get; set; }

    /// <summary>Mã hàng, không được trùng.</summary>
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int UnitId { get; set; }
    public Unit Unit { get; set; } = null!;

    /// <summary>Số lượng tồn hiện tại. Chỉ được thay đổi qua ConfirmImport / ConfirmExport / kiểm kê.</summary>
    public int Quantity { get; set; }

    /// <summary>Định mức tồn tối thiểu, dùng để cảnh báo sắp hết hàng (Thái Anh).</summary>
    public int MinQuantity { get; set; }

    public decimal ImportPrice { get; set; }
    public decimal ExportPrice { get; set; }

    public bool IsActive { get; set; } = true;

    /// <summary>Dùng để phát hiện 2 người cùng sửa 1 mặt hàng (concurrency). SQL Server tự cập nhật.</summary>
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

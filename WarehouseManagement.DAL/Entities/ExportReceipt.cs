namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Thành (bản nháp)
public class ExportReceipt
{
    public int Id { get; set; }

    /// <summary>Số phiếu, ví dụ PX0001.</summary>
    public string Code { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    /// <summary>Người lập phiếu.</summary>
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public decimal TotalAmount { get; set; }
    public string? Note { get; set; }

    public ICollection<ExportReceiptDetail> Details { get; set; } = new List<ExportReceiptDetail>();
}

public class ExportReceiptDetail
{
    public int Id { get; set; }

    public int ExportReceiptId { get; set; }
    public ExportReceipt ExportReceipt { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

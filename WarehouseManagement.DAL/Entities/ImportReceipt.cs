namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Đức (bản nháp)
public class ImportReceipt
{
    public int Id { get; set; }

    /// <summary>Số phiếu, ví dụ PN0001.</summary>
    public string Code { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    /// <summary>Người lập phiếu.</summary>
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public decimal TotalAmount { get; set; }
    public string? Note { get; set; }

    public ICollection<ImportReceiptDetail> Details { get; set; } = new List<ImportReceiptDetail>();
}

public class ImportReceiptDetail
{
    public int Id { get; set; }

    public int ImportReceiptId { get; set; }
    public ImportReceipt ImportReceipt { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

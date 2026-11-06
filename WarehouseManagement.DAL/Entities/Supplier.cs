namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Đức (bản nháp)
public class Supplier
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }

    public ICollection<ImportReceipt> ImportReceipts { get; set; } = new List<ImportReceipt>();
}

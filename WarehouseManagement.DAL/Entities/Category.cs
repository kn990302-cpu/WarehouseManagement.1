namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Thuận (bản nháp, Thuận bổ sung thêm trường nếu cần)
public class Category
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

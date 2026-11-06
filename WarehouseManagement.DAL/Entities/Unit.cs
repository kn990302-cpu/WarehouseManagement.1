namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Thuận. Đơn vị tính: Cái, Hộp, Thùng, Kg...
public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

namespace WarehouseManagement.DAL.Entities;

// Người phụ trách: Thái Anh (bản nháp)
public class Stocktake
{
    public int Id { get; set; }

    /// <summary>Số phiếu, ví dụ KK0001.</summary>
    public string Code { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    public string? Note { get; set; }

    public ICollection<StocktakeDetail> Details { get; set; } = new List<StocktakeDetail>();
}

public class StocktakeDetail
{
    public int Id { get; set; }

    public int StocktakeId { get; set; }
    public Stocktake Stocktake { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    /// <summary>Số lượng theo hệ thống tại thời điểm kiểm kê.</summary>
    public int SystemQuantity { get; set; }

    /// <summary>Số lượng đếm thực tế.</summary>
    public int ActualQuantity { get; set; }

    /// <summary>Chênh lệch = Thực tế - Hệ thống.</summary>
    public int Difference => ActualQuantity - SystemQuantity;

    public string? Reason { get; set; }
}

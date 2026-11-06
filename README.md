# Phần mềm Quản lý Kho hàng (WPF + EF Core)

Đồ án nhóm 5 người, 4 tuần. Kiến trúc 3 lớp: **UI (WPF)** → **BLL** → **DAL (EF Core Code-First)**.

## 1. Yêu cầu môi trường

| Công cụ | Ghi chú |
|---|---|
| Visual Studio 2026 Insiders (Community) | Workload **.NET desktop development** |
| .NET 10 SDK | Đi kèm VS 2026 |
| SQL Server Express **LocalDB** | Tick trong VS Installer → Individual components |
| SSMS (tùy chọn) | Xem database: server `(localdb)\MSSQLLocalDB` |
| Git | Đi kèm VS hoặc cài Git for Windows |

Kiểm tra nhanh trong Developer PowerShell: `dotnet --list-sdks`, `sqllocaldb info`, `git --version`.

## 2. Chạy lần đầu (thành viên)

1. Clone repo, mở `WarehouseManagement.sln`.
2. Chuột phải `WarehouseManagement.UI` → **Set as Startup Project** (thường đã mặc định).
3. Nhấn **F5**. Ứng dụng tự tạo database `WarehouseManagementDb` và áp dụng migration mới nhất.
4. Đăng nhập: **admin / admin123** (đổi mật khẩu sau khi demo).

> Máy bạn dùng SQL Express thay vì LocalDB? Copy `WarehouseManagement.UI/appsettings.Local.example.json` thành `appsettings.Local.json` và sửa chuỗi kết nối. File này không bị đẩy lên Git.

## 3. Cấu trúc thư mục

```
WarehouseManagement.sln
├─ Directory.Packages.props        ← phiên bản NuGet chung (chỉ Khang sửa)
├─ WarehouseManagement.DAL         ← Lớp dữ liệu
│  ├─ Entities/                    ← Class bảng (mỗi người sửa file của mình)
│  ├─ Configurations/              ← Cấu hình bảng, 1 file cho mỗi người
│  ├─ Context/AppDbContext.cs      ← ĐÃ khai báo sẵn mọi DbSet, không cần sửa
│  └─ Migrations/                  ← Sinh tự động bằng Add-Migration
├─ WarehouseManagement.BLL         ← Nghiệp vụ
│  ├─ Auth/                        ← Đăng nhập, phiên, băm mật khẩu (Khang)
│  ├─ Security/Permissions.cs      ← Bảng phân quyền theo vai trò
│  └─ Services/                    ← Mỗi người tạo Service của mình ở đây
└─ WarehouseManagement.UI          ← Giao diện WPF + MaterialDesign
   ├─ MainWindow.xaml              ← Khung điều hướng (không cần sửa)
   ├─ Navigation/                  ← Menu đã đăng ký sẵn cho 5 người
   └─ Views/
      ├─ Auth/      Khang     (Đăng nhập)
      ├─ Admin/     Khang     (Người dùng)
      ├─ Catalog/   Thuận     (Hàng hóa, Loại hàng, Pop-up chọn hàng)
      ├─ Inbound/   Đức       (Nhà cung cấp, Nhập kho)
      ├─ Outbound/  Thành     (Khách hàng, Xuất kho)
      └─ Reports/   Thái Anh  (Tổng quan, Kiểm kê, Báo cáo)
```

## 4. Ai làm ở đâu

| Thành viên | Nhánh | Entities / Configurations | BLL/Services | UI/Views |
|---|---|---|---|---|
| Khang | `feature/auth` | User, Role / `SystemConfigurations.cs` | `Auth/` | `Auth/`, `Admin/` |
| Thuận | `feature/catalog` | Category, Unit, Product / `CatalogConfigurations.cs` | `ProductService`, `CategoryService` | `Catalog/` |
| Đức | `feature/inbound` | Supplier, ImportReceipt / `InboundConfigurations.cs` | `SupplierService`, `ImportService` | `Inbound/` |
| Thành | `feature/outbound` | Customer, ExportReceipt / `OutboundConfigurations.cs` | `CustomerService`, `ExportService` | `Outbound/` |
| Thái Anh | `feature/reports` | Stocktake / `StocktakeConfigurations.cs` | `StocktakeService`, `ReportService` | `Reports/` |

Mẫu viết Service trong BLL:

```csharp
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.DAL.Context;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.BLL.Services;

public class ProductService
{
    public List<Product> GetAll()
    {
        using var db = DbContextProvider.Create();
        return db.Products.Include(p => p.Category).ToList();
    }
}
```

Lấy người đang đăng nhập (ví dụ để ghi người lập phiếu): `UserSession.RequireUser().Id`.

## 5. Quy tắc làm việc nhóm

**Git**
- Không ai push thẳng lên `main`. Làm trên nhánh của mình, xong chức năng thì tạo **Pull Request** để Khang review.
- Trước khi bắt đầu làm mỗi ngày: merge `main` mới nhất vào nhánh của mình.
- PR nhỏ, merge thường xuyên. Không dồn đến tuần 4.

**Migration (quan trọng, dễ conflict nhất)**
1. Pull/merge `main` mới nhất trước.
2. Sửa Entity / Configuration của mình.
3. Mở **Package Manager Console**, chọn *Default project* = `WarehouseManagement.DAL`, chạy:
   `Add-Migration Catalog_ThemTruongMoTa` (đặt tên có tiền tố module).
4. Chạy ứng dụng để kiểm tra, rồi tạo PR **ngay**.
5. Nếu PR bị conflict ở `AppDbContextModelSnapshot.cs`: `Remove-Migration`, merge `main`, `Add-Migration` lại.

**Không tự sửa** (báo Khang): `AppDbContext.cs`, `MainWindow.xaml`, `NavigationRegistry.cs`, `Directory.Packages.props`, `.csproj`.

**Thêm NuGet package**: báo Khang thêm phiên bản vào `Directory.Packages.props`, sau đó trong `.csproj` chỉ ghi `<PackageReference Include="TenPackage" />` (không ghi Version).

## 6. Vai trò mặc định

| Mã | Tên | Được dùng |
|---|---|---|
| ADMIN | Quản trị viên | Tất cả |
| MANAGER | Quản lý | Tất cả trừ Người dùng |
| KEEPER | Thủ kho | Tất cả trừ Người dùng và Báo cáo |

Sửa quyền trong `WarehouseManagement.BLL/Security/Permissions.cs`, menu tự ẩn/hiện theo.

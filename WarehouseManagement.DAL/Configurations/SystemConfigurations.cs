using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WarehouseManagement.DAL.Entities;

namespace WarehouseManagement.DAL.Configurations;

// Người phụ trách: Khang

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

        // Dữ liệu vai trò cố định (giá trị phải là hằng số, không dùng DateTime.Now ở đây)
        builder.HasData(
            new Role { Id = RoleIds.Admin, Code = RoleCodes.Admin, Name = "Quản trị viên" },
            new Role { Id = RoleIds.Manager, Code = RoleCodes.Manager, Name = "Quản lý" },
            new Role { Id = RoleIds.WarehouseKeeper, Code = RoleCodes.WarehouseKeeper, Name = "Thủ kho" });
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.PasswordHash).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();

        builder.HasOne(x => x.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

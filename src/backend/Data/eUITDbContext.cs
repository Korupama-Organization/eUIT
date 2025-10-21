using Microsoft.EntityFrameworkCore;
using eUIT.API.Models;

namespace eUIT.API.Data;

public class eUITDbContext : DbContext
{
    public eUITDbContext(DbContextOptions<eUITDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet cho quản lý Refresh Tokens
    /// Bảng này sẽ được tạo thủ công trong database bằng script SQL
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình cho RefreshToken entity
        // Lưu ý: Bảng và index đã được tạo sẵn trong database bằng script SQL
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            // Chỉ định tên bảng (đã tồn tại)
            entity.ToTable("refresh_tokens");
            
            // Mapping các cột (tên cột đã được định nghĩa trong attributes)
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TokenHash).HasColumnName("token_hash");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IsRevoked).HasColumnName("is_revoked");
            entity.Property(e => e.RevokedAt).HasColumnName("revoked_at");
            entity.Property(e => e.DeviceInfo).HasColumnName("device_info");
            entity.Property(e => e.IpAddress).HasColumnName("ip_address");
            
            // Không tạo index ở đây vì đã tạo trong SQL script
            // Index được tạo thủ công trong database:
            // - ix_refresh_tokens_token_hash (unique)
            // - ix_refresh_tokens_user_id  
            // - ix_refresh_tokens_expires_at
            // - ix_refresh_tokens_cleanup (composite: is_revoked, expires_at)
        });
    }
}


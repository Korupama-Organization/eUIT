using Microsoft.EntityFrameworkCore;

namespace eUIT.API.Data;

public class eUITDbContext : DbContext
{
    public eUITDbContext(DbContextOptions<eUITDbContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }              // Bảng Users
    public DbSet<RefreshToken> RefreshTokens { get; set; } // Bảng RefreshTokens
}

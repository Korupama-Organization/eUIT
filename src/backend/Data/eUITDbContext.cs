using Microsoft.EntityFrameworkCore;
using eUIT.API.Models;

namespace eUIT.API.Data;

public class eUITDbContext : DbContext
{
    public eUITDbContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// DbSet cho quản lý Refresh Tokens
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    /// <summary>
    /// DbSet cho quản lý Môn học
    /// </summary>
    public DbSet<Courses> Courses { get; set; }

    /// <summary>
    /// DbSet cho quản lý Báo nghỉ dạy của Giảng viên
    /// </summary>
    public DbSet<Absence> bao_nghi_day { get; set; }

    /// <summary>
    /// DbSet cho quản lý Thông báo
    /// </summary>
    public DbSet<Announcement> thong_bao { get; set; }

    /// <summary>
    /// DbSet cho quản lý Sinh viên
    /// </summary>
    public DbSet<Student> sinh_vien { get; set; }

     /// <summary>
    /// DbSet cho quản lý Thông báo cá nhân cho Giảng viên
    /// </summary>
    public DbSet<Notification> thong_bao_giang_vien { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Cấu hình cho RefreshToken entity
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");
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
        });

        // Cấu hình cho Absence entity (bao_nghi_day)
        modelBuilder.Entity<Absence>(entity =>
        {
            entity.ToTable("bao_nghi_day");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaLop).HasColumnName("ma_lop").HasMaxLength(20);
            entity.Property(e => e.MaGiangVien).HasColumnName("ma_giang_vien").HasMaxLength(5);
            entity.Property(e => e.LyDo).HasColumnName("ly_do").HasMaxLength(200);
            entity.Property(e => e.NgayNghi).HasColumnName("ngay_nghi").HasColumnType("date");
            entity.Property(e => e.TinhTrang).HasColumnName("tinh_trang").HasMaxLength(20);
            entity.HasKey(e => e.Id);
        });

        // Cấu hình cho Announcement entity (thong_bao)
        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.ToTable("thong_bao");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TieuDe).HasColumnName("tieu_de").HasMaxLength(100);
            entity.Property(e => e.NoiDung).HasColumnName("noi_dung").HasColumnType("text");
            entity.Property(e => e.NgayTao).HasColumnName("ngay_tao").HasColumnType("date");
            entity.Property(e => e.NgayCapNhat).HasColumnName("ngay_cap_nhat").HasColumnType("date");
            entity.HasKey(e => e.Id);
        });

        //Cấu hình cho Student entity (sinh_vien)
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("sinh_vien");

            // Primary Key
            entity.HasKey(e => e.Mssv);
            entity.Property(e => e.Mssv).HasColumnName("mssv");

            // Thông tin cơ bản
            entity.Property(e => e.HoTen).HasColumnName("ho_ten");
            entity.Property(e => e.NgaySinh).HasColumnName("ngay_sinh").HasColumnType("date");
            entity.Property(e => e.NganhHoc).HasColumnName("nganh_hoc");
            entity.Property(e => e.KhoaHoc).HasColumnName("khoa_hoc");
            entity.Property(e => e.LopSinhHoat).HasColumnName("lop_sinh_hoat");
            entity.Property(e => e.NoiSinh).HasColumnName("noi_sinh");
            entity.Property(e => e.Cccd).HasColumnName("cccd");
            entity.Property(e => e.NgayCapCccd).HasColumnName("ngay_cap_cccd").HasColumnType("date");
            entity.Property(e => e.NoiCapCccd).HasColumnName("noi_cap_cccd");
            entity.Property(e => e.DanToc).HasColumnName("dan_toc");
            entity.Property(e => e.TonGiao).HasColumnName("ton_giao");
            entity.Property(e => e.SoDienThoai).HasColumnName("so_dien_thoai");
            entity.Property(e => e.DiaChiThuongTru).HasColumnName("dia_chi_thuong_tru");
            entity.Property(e => e.TinhThanhPho).HasColumnName("tinh_thanh_pho");
            entity.Property(e => e.PhuongXa).HasColumnName("phuong_xa");
            entity.Property(e => e.QuaTrinhHocTapCongTac).HasColumnName("qua_trinh_hoc_tap_cong_tac").HasColumnType("text");
            entity.Property(e => e.ThanhTich).HasColumnName("thanh_tich");
            entity.Property(e => e.EmailCaNhan).HasColumnName("email_ca_nhan");
            entity.Property(e => e.MaNganHang).HasColumnName("ma_ngan_hang");
            entity.Property(e => e.TenNganHang).HasColumnName("ten_ngan_hang");

            // Thông tin tài khoản ngân hàng
            entity.Property(e => e.SoTaiKhoan).HasColumnName("so_tai_khoan");
            entity.Property(e => e.ChiNhanh).HasColumnName("chi_nhanh");

            // Thông tin cha
            entity.Property(e => e.HoTenCha).HasColumnName("ho_ten_cha");
            entity.Property(e => e.QuocTichCha).HasColumnName("quoc_tich_cha");
            entity.Property(e => e.DanTocCha).HasColumnName("dan_toc_cha");
            entity.Property(e => e.TonGiaoCha).HasColumnName("ton_giao_cha");
            entity.Property(e => e.SdtCha).HasColumnName("sdt_cha");
            entity.Property(e => e.EmailCha).HasColumnName("email_cha");
            entity.Property(e => e.DiaChiThuongTruCha).HasColumnName("dia_chi_thuong_tru_cha");
            entity.Property(e => e.CongViecCha).HasColumnName("cong_viec_cha");

            // Thông tin mẹ
            entity.Property(e => e.HoTenMe).HasColumnName("ho_ten_me");
            entity.Property(e => e.QuocTichMe).HasColumnName("quoc_tich_me");
            entity.Property(e => e.DanTocMe).HasColumnName("dan_toc_me");
            entity.Property(e => e.TonGiaoMe).HasColumnName("ton_giao_me");
            entity.Property(e => e.SdtMe).HasColumnName("sdt_me");
            entity.Property(e => e.EmailMe).HasColumnName("email_me");
            entity.Property(e => e.DiaChiThuongTruMe).HasColumnName("dia_chi_thuong_tru_me");
            entity.Property(e => e.CongViecMe).HasColumnName("cong_viec_me");

            // Thông tin người giám hộ
            entity.Property(e => e.HoTenNgh).HasColumnName("ho_ten_ngh");
            entity.Property(e => e.QuocTichNgh).HasColumnName("quoc_tich_ngh");
            entity.Property(e => e.DanTocNgh).HasColumnName("dan_toc_ngh");
            entity.Property(e => e.TonGiaoNgh).HasColumnName("ton_giao_ngh");
            entity.Property(e => e.SdtNgh).HasColumnName("sdt_ngh");
            entity.Property(e => e.EmailNgh).HasColumnName("email_ngh");
            entity.Property(e => e.DiaChiThuongTruNgh).HasColumnName("dia_chi_thuong_tru_ngh");
            entity.Property(e => e.CongViecNgh).HasColumnName("cong_viec_ngh");

            // Thông tin khẩn cấp
            entity.Property(e => e.ThongTinNguoiCanBaoTin).HasColumnName("thong_tin_nguoi_can_bao_tin");
            entity.Property(e => e.SoDienThoaiBaoTin).HasColumnName("so_dien_thoai_bao_tin");

            // Ảnh thẻ
            entity.Property(e => e.AnhTheUrl)
                    .HasColumnName("anh_the_url")
                    .HasDefaultValue("default_url");
        });
    
          //Cấu hình cho Notification entity (thong_bao_giang_vien)
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("thong_bao_giang_vien");

            // Primary Key
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");

            // Mapping các cột
            entity.Property(e => e.TieuDe).HasColumnName("tieu_de").HasMaxLength(255);
            entity.Property(e => e.NoiDung).HasColumnName("noi_dung").HasColumnType("text");
            entity.Property(e => e.NgayGui).HasColumnName("ngay_gui").HasColumnType("timestamp without time zone");
            entity.Property(e => e.NguoiGuiId).HasColumnName("nguoi_gui_id");
        });
    }
}

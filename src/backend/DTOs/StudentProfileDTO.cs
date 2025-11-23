
public class StudentProfileDto
{
    // Thông tin cơ bản
    public int Mssv { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public DateOnly NgaySinh { get; set; } 
    public string NganhHoc { get; set; } = string.Empty;
    public int KhoaHoc { get; set; }
    public string LopSinhHoat { get; set; } = string.Empty;
    
    // Thông tin cá nhân chi tiết
    public string? NoiSinh { get; set; }
    public string? Cccd { get; set; }
    public DateOnly? NgayCapCccd { get; set; }
    public string? NoiCapCccd { get; set; }
    public string? DanToc { get; set; }
    public string? TonGiao { get; set; }
    public string? SoDienThoai { get; set; }
    public string? DiaChiThuongTru { get; set; }
    public string? TinhThanhPho { get; set; }
    public string? PhuongXa { get; set; }
    public string? QuaTrinhHocTapCongTac { get; set; }
    public string? ThanhTich { get; set; }
    public string? EmailCaNhan { get; set; }
    
    // Thông tin ngân hàng
    public string? MaNganHang { get; set; }
    public string? TenNganHang { get; set; }
    public string? SoTaiKhoan { get; set; }
    public string? ChiNhanh { get; set; }
    
    // Thông tin Cha
    public ThongTinPhuHuynh? Cha { get; set; }
    
    // Thông tin Mẹ
    public ThongTinPhuHuynh? Me { get; set; }
    
    // Thông tin Người giám hộ
    public ThongTinPhuHuynh? NguoiGiamHo { get; set; } 
    
    // Thông tin Người cần báo tin
    public string? ThongTinNguoiCanBaoTin { get; set; }
    public string? SoDienThoaiBaoTin { get; set; }
    
    // Ảnh thẻ (URL đầy đủ)
    public string? AvatarFullUrl { get; set; }
}

// Lớp nội bộ để nhóm thông tin Phụ Huynh/Giám hộ
public class ThongTinPhuHuynh
{
    public string? HoTen { get; set; }
    public string? QuocTich { get; set; }
    public string? DanToc { get; set; }
    public string? TonGiao { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }
    public string? DiaChiThuongTru { get; set; }
    public string? CongViec { get; set; }
}
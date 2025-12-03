namespace eUIT.API.DTOs;

/// <summary>
/// DTO đầy đủ để trả về thông tin chi tiết sinh viên
/// Sử dụng cho endpoint GET /api/GiangVien/students/{studentId}
/// </summary>
public class StudentDTO
{
    // Thông tin cơ bản
    public int Mssv { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public DateTime NgaySinh { get; set; }
    public string NganhHoc { get; set; } = string.Empty;
    public int KhoaHoc { get; set; }
    public string LopSinhHoat { get; set; } = string.Empty;
    public string NoiSinh { get; set; } = string.Empty;
    
    // Giấy tờ tùy thân
    public string Cccd { get; set; } = string.Empty;
    public DateTime NgayCapCccd { get; set; }
    public string NoiCapCccd { get; set; } = string.Empty;
    
    // Thông tin cá nhân
    public string DanToc { get; set; } = string.Empty;
    public string TonGiao { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string DiaChiThuongTru { get; set; } = string.Empty;
    public string TinhThanhPho { get; set; } = string.Empty;
    public string PhuongXa { get; set; } = string.Empty;
    
    // Thông tin học tập
    public string QuaTrinhHocTapCongTac { get; set; } = string.Empty;
    public string ThanhTich { get; set; } = string.Empty;
    public string EmailCaNhan { get; set; } = string.Empty;
    
    // Thông tin ngân hàng
    public string MaNganHang { get; set; } = string.Empty;
    public string TenNganHang { get; set; } = string.Empty;
    public string SoTaiKhoan { get; set; } = string.Empty;
    public string ChiNhanh { get; set; } = string.Empty;
    
    // Thông tin cha
    public string HoTenCha { get; set; } = string.Empty;
    public string QuocTichCha { get; set; } = string.Empty;
    public string DanTocCha { get; set; } = string.Empty;
    public string TonGiaoCha { get; set; } = string.Empty;
    public string SdtCha { get; set; } = string.Empty;
    public string EmailCha { get; set; } = string.Empty;
    public string DiaChiThuongTruCha { get; set; } = string.Empty;
    public string CongViecCha { get; set; } = string.Empty;
    
    // Thông tin mẹ
    public string HoTenMe { get; set; } = string.Empty;
    public string QuocTichMe { get; set; } = string.Empty;
    public string DanTocMe { get; set; } = string.Empty;
    public string TonGiaoMe { get; set; } = string.Empty;
    public string SdtMe { get; set; } = string.Empty;
    public string EmailMe { get; set; } = string.Empty;
    public string DiaChiThuongTruMe { get; set; } = string.Empty;
    public string CongViecMe { get; set; } = string.Empty;
    
    // Thông tin người giám hộ
    public string? HoTenNgh { get; set; } = string.Empty;
    public string? QuocTichNgh { get; set; } = string.Empty;
    public string? DanTocNgh { get; set; } = string.Empty;
    public string? TonGiaoNgh { get; set; } = string.Empty;
    public string? SdtNgh { get; set; } = string.Empty;
    public string? EmailNgh { get; set; } = string.Empty;
    public string? DiaChiThuongTruNgh { get; set; } = string.Empty;
    public string? CongViecNgh { get; set; } = string.Empty;
    
    // Thông tin khẩn cấp
    public string ThongTinNguoiCanBaoTin { get; set; } = string.Empty;
    public string SoDienThoaiBaoTin { get; set; } = string.Empty;
    
    // Ảnh thẻ
    public string? AnhTheUrl { get; set; } = string.Empty;
}

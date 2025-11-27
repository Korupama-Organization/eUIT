namespace eUIT.API.Models;

/// <summary>
/// Model cho bảng sinh_vien - Quản lý thông tin sinh viên
/// </summary>
public class Student
{
    /// <summary>
    /// Mã số sinh viên (Primary Key)
    /// </summary>
    public int Mssv { get; set; }

    /// <summary>
    /// Họ tên sinh viên
    /// </summary>
    public string HoTen { get; set; } = string.Empty;

    /// <summary>
    /// Ngày sinh
    /// </summary>
    public DateTime NgaySinh { get; set; }

    /// <summary>
    /// Ngành học
    /// </summary>
    public string NganhHoc { get; set; } = string.Empty;

    /// <summary>
    /// Khóa học
    /// </summary>
    public int KhoaHoc { get; set; }

    /// <summary>
    /// Lớp sinh hoạt
    /// </summary>
    public string LopSinhHoat { get; set; } = string.Empty;

    /// <summary>
    /// Nơi sinh
    /// </summary>
    public string NoiSinh { get; set; } = string.Empty;

    /// <summary>
    /// Căn cước công dân
    /// </summary>
    public string Cccd { get; set; } = string.Empty;

    /// <summary>
    /// Ngày cấp CCCD
    /// </summary>
    public DateTime NgayCapCccd { get; set; }

    /// <summary>
    /// Nơi cấp CCCD
    /// </summary>
    public string NoiCapCccd { get; set; } = string.Empty;

    /// <summary>
    /// Dân tộc
    /// </summary>
    public string DanToc { get; set; } = string.Empty;

    /// <summary>
    /// Tôn giáo
    /// </summary>
    public string TonGiao { get; set; } = string.Empty;

    /// <summary>
    /// Số điện thoại
    /// </summary>
    public string SoDienThoai { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ thường trú
    /// </summary>
    public string DiaChiThuongTru { get; set; } = string.Empty;

    /// <summary>
    /// Tỉnh thành phố
    /// </summary>
    public string TinhThanhPho { get; set; } = string.Empty;

    /// <summary>
    /// Phường xã
    /// </summary>
    public string PhuongXa { get; set; } = string.Empty;

    /// <summary>
    /// Quá trình học tập công tác (text)
    /// </summary>
    public string QuaTrinhHocTapCongTac { get; set; } = string.Empty;

    /// <summary>
    /// Thành tích
    /// </summary>
    public string ThanhTich { get; set; } = string.Empty;

    /// <summary>
    /// Email cá nhân
    /// </summary>
    public string EmailCaNhan { get; set; } = string.Empty;

    /// <summary>
    /// Mã ngân hàng
    /// </summary>
    public string MaNganHang { get; set; } = string.Empty;

    /// <summary>
    /// Tên ngân hàng
    /// </summary>
    public string TenNganHang { get; set; } = string.Empty;

    /// <summary>
    /// Số tài khoản
    /// </summary>
    public string SoTaiKhoan { get; set; } = string.Empty;

    /// <summary>
    /// Chi nhánh
    /// </summary>
    public string ChiNhanh { get; set; } = string.Empty;

    /// <summary>
    /// Họ tên cha
    /// </summary>
    public string HoTenCha { get; set; } = string.Empty;

    /// <summary>
    /// Quốc tịch cha
    /// </summary>
    public string QuocTichCha { get; set; } = string.Empty;

    /// <summary>
    /// Dân tộc cha
    /// </summary>
    public string DanTocCha { get; set; } = string.Empty;

    /// <summary>
    /// Tôn giáo cha
    /// </summary>
    public string TonGiaoCha { get; set; } = string.Empty;

    /// <summary>
    /// Số điện thoại cha
    /// </summary>
    public string SdtCha { get; set; } = string.Empty;

    /// <summary>
    /// Email cha
    /// </summary>
    public string EmailCha { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ thường trú cha
    /// </summary>
    public string DiaChiThuongTruCha { get; set; } = string.Empty;

    /// <summary>
    /// Công việc cha
    /// </summary>
    public string CongViecCha { get; set; } = string.Empty;

    /// <summary>
    /// Họ tên mẹ
    /// </summary>
    public string HoTenMe { get; set; } = string.Empty;

    /// <summary>
    /// Quốc tịch mẹ
    /// </summary>
    public string QuocTichMe { get; set; } = string.Empty;

    /// <summary>
    /// Dân tộc mẹ
    /// </summary>
    public string DanTocMe { get; set; } = string.Empty;

    /// <summary>
    /// Tôn giáo mẹ
    /// </summary>
    public string TonGiaoMe { get; set; } = string.Empty;

    /// <summary>
    /// Số điện thoại mẹ
    /// </summary>
    public string SdtMe { get; set; } = string.Empty;

    /// <summary>
    /// Email mẹ
    /// </summary>
    public string EmailMe { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ thường trú mẹ
    /// </summary>
    public string DiaChiThuongTruMe { get; set; } = string.Empty;

    /// <summary>
    /// Công việc mẹ
    /// </summary>
    public string CongViecMe { get; set; } = string.Empty;

    /// <summary>
    /// Họ tên người giám hộ (nếu có)
    /// </summary>
    public string? HoTenNgh { get; set; } = string.Empty;

    /// <summary>
    /// Quốc tịch người giám hộ
    /// </summary>
    public string? QuocTichNgh { get; set; } = string.Empty;

    /// <summary>
    /// Dân tộc người giám hộ
    /// </summary>
    public string? DanTocNgh { get; set; } = string.Empty;

    /// <summary>
    /// Tôn giáo người giám hộ
    /// </summary>
    public string? TonGiaoNgh { get; set; } = string.Empty;

    /// <summary>
    /// Số điện thoại người giám hộ
    /// </summary>
    public string? SdtNgh { get; set; } = string.Empty;

    /// <summary>
    /// Email người giám hộ
    /// </summary>
    public string? EmailNgh { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ thường trú người giám hộ
    /// </summary>
    public string? DiaChiThuongTruNgh { get; set; } = string.Empty;

    /// <summary>
    /// Công việc người giám hộ
    /// </summary>
    public string? CongViecNgh { get; set; } = string.Empty;

    /// <summary>
    /// Thông tin người cần báo tin (trường hợp khẩn cấp)
    /// </summary>
    public string ThongTinNguoiCanBaoTin { get; set; } = string.Empty;

    /// <summary>
    /// Số điện thoại báo tin
    /// </summary>
    public string SoDienThoaiBaoTin { get; set; } = string.Empty;

    /// <summary>
    /// Ảnh thẻ URL
    /// </summary>
    public string? AnhTheUrl { get; set; } = string.Empty;
}

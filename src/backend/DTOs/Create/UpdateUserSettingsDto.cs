namespace eUIT.API.DTOs.Create
{
    /// <summary>
    /// DTO cho việc cập nhật cài đặt người dùng
    /// </summary>
    public class UpdateUserSettingsDto
    {
        // Màn hình
        public bool? CheDoToi { get; set; }
        
        // Thông báo đẩy
        public bool? CapNhatKetQuaHocTap { get; set; }
        public bool? ThongBaoNghiLop { get; set; }
        public bool? ThongBaoHocBu { get; set; }
        public bool? LichThi { get; set; }
        public bool? ThongBaoMoi { get; set; }
        public bool? CapNhatTrangThaiThuTucHanhChinh { get; set; }
        
        // Thông báo email
        public bool? BatThongBaoEmail { get; set; }
    }
}
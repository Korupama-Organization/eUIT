namespace eUIT.API.DTOs
{
    public class ReExamDto
    {
        public int Mssv { get; set; }
        public string MaMon { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;
        public DateTime NgayThi { get; set; }
        public string PhongThi { get; set; } = string.Empty;
        public int CaThi { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public DateTime? NgayDangKy { get; set; }
    }
}

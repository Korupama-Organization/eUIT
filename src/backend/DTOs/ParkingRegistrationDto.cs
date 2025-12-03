namespace eUIT.API.DTOs
{
    public class ParkingRegistrationDto
    {
        public int Mssv { get; set; }
        public string MaBienSo { get; set; } = string.Empty;
        public double SoThang { get; set; }
        public decimal SoTien { get; set; }
        public string TinhTrang { get; set; } = string.Empty;
        public DateTime NgayDangKy { get; set; }
        public DateTime NgayHetHan { get; set; }
    }
}

namespace eUIT.API.DTOs
{
    public class TranscriptRequestDto
    {
        public int Mssv { get; set; }
        public string LoaiBangDiem { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public string NgonNgu { get; set; } = string.Empty;
        public DateTime NgayDangKy { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public string ChiPhi { get; set; } = string.Empty;
    }
}

namespace eUIT.API.DTOs
{
    public class IntroductionLetterDto
    {
        public int Mssv { get; set; }
        public string NoiNhan { get; set; } = string.Empty;
        public string MucDich { get; set; } = string.Empty;
        public DateTime NgayDangKy { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}

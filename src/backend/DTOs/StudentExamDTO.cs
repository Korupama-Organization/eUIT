using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs
{
    public class StudentExamDto
    {
        public string MaMonHoc { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;
        public string MaGiangVien { get; set; } = string.Empty;
        public DateTime NgayThi { get; set; }
        public int CaThi { get; set; }
        public string PhongThi { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
    }
}

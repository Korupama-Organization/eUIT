using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace eUIT.API.Models
{
    [Keyless]  // EF không yêu cầu PK
    public class Courses
    {
        public string MaMonHoc { get; set; } = string.Empty;
        public string TenMonHocVn { get; set; } = string.Empty;
        public string TenMonHocEn { get; set; } = string.Empty;
        public string ConMoLop { get; set; } = string.Empty;
        public string KhoaBoMonQuanLy { get; set; } = string.Empty;
        public string LoaiMonHoc { get; set; } = string.Empty;
        public int SoTcLyThuyet { get; set; }
        public int SoTcThucHanh { get; set; }
    }
}

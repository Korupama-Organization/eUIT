using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUIT.API.Models
{
    public class Student
    {
        [Key]
        public int Mssv { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string? AnhTheUrl { get; set; }
        // Thêm các cột khác nếu cần
    }
}

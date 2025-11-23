using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUIT.API.Models
{
    [Table("bao_nghi_day")]
    public class Absence
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("ma_lop")]
        [StringLength(20)]
        [Required]
        public string MaLop { get; set; } = string.Empty;

        [Column("ma_giang_vien")]
        [StringLength(5)]
        [Required]
        public string MaGiangVien { get; set; } = string.Empty;

        [Column("ly_do")]
        [StringLength(200)]
        [Required]
        public string LyDo { get; set; } = string.Empty;

        [Column("ngay_nghi")]
        [Required]
        public DateTime NgayNghi { get; set; }

        [Column("tinh_trang")]
        [StringLength(20)]
        public string? TinhTrang { get; set; }
    }
}
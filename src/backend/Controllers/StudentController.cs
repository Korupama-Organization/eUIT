using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eUIT.API.Data;
using eUIT.API.DTOs;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema; // Thêm thư viện này

namespace eUIT.API.Controllers;

[Authorize] // Yêu cầu tất cả các API trong controller này đều phải được xác thực
[ApiController]
[Route("api/[controller]")] // Đường dẫn sẽ là /api/students
public class StudentsController : ControllerBase
{
    private readonly eUITDbContext _context;

    public StudentsController(eUITDbContext context)
    {
        _context = context;
    }

    // --- Các lớp Model/Kết quả truy vấn nội bộ (Internal Query Result Classes) ---
    // Các lớp này ánh xạ tới kết quả trả về từ hàm SQL trong database (thường là snake_case)
    // Các thuộc tính trong lớp này được đổi thành PascalCase để đồng nhất với DTO và sử dụng [Column] để chỉ định tên cột DB tương ứng.

    // Dùng để ánh xạ kết quả từ func_get_next_class
    private class NextClassInfo
    {
        [Column("ma_lop")]
        public string MaLop { get; set; } = string.Empty;
        [Column("ten_mon_hoc_vn")]
        public string TenMonHocVn { get; set; } = string.Empty;
        [Column("thu")]
        public string Thu { get; set; } = string.Empty;
        [Column("tiet_bat_dau")]
        public int TietBatDau { get; set; }
        [Column("tiet_ket_thuc")]
        public int TietKetThuc { get; set; }
        [Column("phong_hoc")]
        public string PhongHoc { get; set; } = string.Empty;
        [Column("ngay_hoc")]
        public DateTime NgayHoc { get; set; }
        [Column("ten_giang_vien")]
        public string TenGiangVien { get; set; } = string.Empty;
    }

    // Dùng để ánh xạ kết quả từ func_get_student_card_info
    private class CardInfoResult
    {
        [Column("mssv")]
        public int Mssv { get; set; }
        [Column("ho_ten")]
        public string HoTen { get; set; } = string.Empty;
        [Column("khoa_hoc")]
        public int KhoaHoc { get; set; }
        [Column("nganh_hoc")]
        public string NganhHoc { get; set; } = string.Empty;
        [Column("anh_the_url")]
        public string? AnhTheUrl { get; set; }
    }

    // Dùng để ánh xạ kết quả từ func_calculate_gpa
    private class QuickGpaResult
    {
        // Chú ý: Dữ liệu mẫu Swagger cho thấy tên cột là "gpa" và "soTinChiTichLuy"
        // 
        [Column("gpa")] 
        public float Gpa { get; set; }
        [Column("so_tin_chi_tich_luy")]
        public int SoTinChiTichLuy { get; set; } = 0;
    }

    // Dùng để ánh xạ kết quả từ chi_tiet_ket_qua_hoc_tap
    private class AcademicResultQueryResult
    {
        // 


        [Column("hoc_ky")]
        public string? HocKy { get; set; }
        [Column("ma_mon_hoc")]
        public string? MaMonHoc { get; set; }
        [Column("ten_mon_hoc")]
        public string? TenMonHoc { get; set; }
        [Column("so_tin_chi")]
        public int? SoTinChi { get; set; }
        [Column("trong_so_qua_trinh")]
        public int? TrongSoQuaTrinh { get; set; }
        [Column("trong_so_giua_ki")]
        public int? TrongSoGiuaKi { get; set; }
        [Column("trong_so_thuc_hanh")]
        public int? TrongSoThucHanh { get; set; }
        [Column("trong_so_cuoi_ki")]
        public int? TrongSoCuoiKi { get; set; }
        [Column("diem_qua_trinh")]
        public decimal? DiemQuaTrinh { get; set; }
        [Column("diem_giua_ki")]
        public decimal? DiemGiuaKi { get; set; }
        [Column("diem_thuc_hanh")]
        public decimal? DiemThucHanh { get; set; }
        [Column("diem_cuoi_ki")]
        public decimal? DiemCuoiKi { get; set; }
        [Column("diem_tong_ket")]
        public decimal? DiemTongKet { get; set; }
    }

    // --- API Endpoints ---

    // GET: /nextclass
    [HttpGet("nextclass")]
    public async Task<ActionResult<NextClassDto>> GetNextClass()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (loggedInMssv == null) return Forbid();

        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var NextClassResult = await
        _context.Database.SqlQuery<NextClassInfo>
        ($"SELECT * FROM func_get_next_class({mssvInt})")
        .FirstOrDefaultAsync();

        if (NextClassResult == null) return NoContent();

        // Ánh xạ từ Query Result sang DTO
        var NextClass = new NextClassDto
        {
            MaLop = NextClassResult.MaLop,
            TenLop = NextClassResult.TenMonHocVn,
            Thu = NextClassResult.Thu,
            TietBatDau = NextClassResult.TietBatDau,
            TietKetThuc = NextClassResult.TietKetThuc,
            PhongHoc = NextClassResult.PhongHoc,
            NgayHoc = NextClassResult.NgayHoc,
            TenGiangVien = NextClassResult.TenGiangVien
        };

        return Ok(NextClass);
    }

    // GET: /card
    [HttpGet("card")]
    public async Task<ActionResult<StudentCardDto>> GetStudentCard()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (loggedInMssv == null) return Forbid();

        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var student = await
            _context.Database.SqlQuery<CardInfoResult>(
            $"SELECT * FROM func_get_student_card_info({mssvInt})")
            .FirstOrDefaultAsync();

        if (student == null) return NotFound();

        string? avatarFullUrl = null;
        if (!string.IsNullOrEmpty(student.AnhTheUrl))
        {
            // Xây dựng URL đầy đủ cho ảnh thẻ
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            avatarFullUrl = $"{baseUrl}/files/{student.AnhTheUrl}"; 
        }

        // Ánh xạ từ Query Result sang DTO
        var studentCard = new StudentCardDto
        {
            Mssv = student.Mssv,
            HoTen = student.HoTen,
            KhoaHoc = student.KhoaHoc,
            NganhHoc = student.NganhHoc,
            AvatarFullUrl = avatarFullUrl
        };

        return Ok(studentCard);
    }

    /// <summary>
    /// Retrieves the quick GPA and accumulated credits for the currently authenticated student.
    /// </summary>
    // GET: /quickgpa
    [HttpGet("quickgpa")]
    public async Task<ActionResult<QuickGpaDto>> GetQuickGpa()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (loggedInMssv == null) return Forbid();

        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var result = await
            _context.Database.SqlQuery<QuickGpaResult>(
            $"SELECT * FROM func_calculate_gpa({mssvInt})")
            .FirstOrDefaultAsync();

        if (result == null) return NotFound();

        // Ánh xạ từ Query Result sang DTO
        var gpaAndCredits = new QuickGpaDto
        {
            Gpa = result.Gpa,
            SoTinChiTichLuy = result.SoTinChiTichLuy
        };

        return Ok(gpaAndCredits);
    }


    // GET: /academicresults
    [HttpGet("academicresults")]
    public async Task<ActionResult<IEnumerable<AcademicResultDTO>>> GetAcademicResults()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (loggedInMssv == null) return Forbid();

        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        // Gọi hàm database để lấy chi tiết kết quả học tập
        var queryResults = await _context.Database.SqlQuery<AcademicResultQueryResult>
            ($"SELECT * FROM chi_tiet_ket_qua_hoc_tap({mssvInt})")
            .ToListAsync();

        if (queryResults == null || queryResults.Count == 0) 
            return NotFound("No academic results found");

        // Chuyển đổi từ query result sang DTO
        var academicResults = queryResults.Select(r => new AcademicResultDTO
        {
            HocKy = r.HocKy ?? string.Empty,
            MaMonHoc = r.MaMonHoc ?? string.Empty,
            TenMonHoc = r.TenMonHoc ?? string.Empty,
            SoTinChi = r.SoTinChi ?? 0,
            TrongSoQuaTrinh = r.TrongSoQuaTrinh ?? 0,
            TrongSoGiuaKi = r.TrongSoGiuaKi ?? 0,
            TrongSoThucHanh = r.TrongSoThucHanh ?? 0,
            TrongSoCuoiKi = r.TrongSoCuoiKi ?? 0,
            DiemQuaTrinh = r.DiemQuaTrinh,
            DiemGiuaKi = r.DiemGiuaKi,
            DiemThucHanh = r.DiemThucHanh,
            DiemCuoiKi = r.DiemCuoiKi,
            DiemTongKet = r.DiemTongKet
        }).ToList();

        return Ok(academicResults);
    }
}

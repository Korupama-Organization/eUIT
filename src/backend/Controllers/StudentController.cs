using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eUIT.API.Data;
using eUIT.API.DTOs;
using System.Security.Claims;

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

    public class NextClassInfo
    {
        public string ma_lop { get; set; }
        public string ten_mon_hoc_vn { get; set; }
        public string thu { get; set; }
        public int tiet_bat_dau { get; set; }
        public int tiet_ket_thuc { get; set; }
        public string phong_hoc { get; set; }
        public DateTime ngay_hoc { get; set; }
    }
    private class CardInfoResult
    {
        public int mssv { get; set; }
        public string ho_ten { get; set; } = string.Empty;
        public int khoa_hoc { get; set; }
        public string nganh_hoc { get; set; } = string.Empty;
        public string? anh_the_url { get; set; }
    }

    private class QuickGpa
    {
        public float gpa { get; set; }

        public int so_tin_chi_tich_luy { get; set; } = 0;
    }
    private class AcademicResultQueryResult
    {
        public string? hoc_ky { get; set; }
        public string? ma_mon_hoc { get; set; }
        public string? ten_mon_hoc { get; set; }
        public int? so_tin_chi { get; set; }
        public int? trong_so_qua_trinh { get; set; }
        public int? trong_so_giua_ki { get; set; }
        public int? trong_so_thuc_hanh { get; set; }
        public int? trong_so_cuoi_ki { get; set; }
        public decimal? diem_qua_trinh { get; set; }
        public decimal? diem_giua_ki { get; set; }
        public decimal? diem_thuc_hanh { get; set; }
        public decimal? diem_cuoi_ki { get; set; }
        public decimal? diem_tong_ket { get; set; }
    }

    // GET: api/students/nextclass
    [HttpGet("/nextclass")]
    public async Task<ActionResult<NextClassDto>> GetNextClass()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (loggedInMssv == null)
        {
            return Forbid();
        }

        if (!int.TryParse(loggedInMssv, out int mssvInt))
        {
            return Forbid();
        }

        var NextClassResult = await
        _context.Database.SqlQuery<NextClassInfo>
        ($"SELECT * FROM func_get_next_class({mssvInt})")
        .FirstOrDefaultAsync();

        if (NextClassResult == null) return NoContent();

        var NextClass = new NextClassDto
        {
            MaLop = NextClassResult.ma_lop,
            TenLop = NextClassResult.ten_mon_hoc_vn,
            Thu = NextClassResult.thu,
            TietBatDau = NextClassResult.tiet_bat_dau,
            TietKetThuc = NextClassResult.tiet_ket_thuc,
            PhongHoc = NextClassResult.phong_hoc,
            NgayHoc = NextClassResult.ngay_hoc
        };

        return Ok(NextClass);
    }

    // GET: api/students/card
    [HttpGet("/card")]
    public async Task<ActionResult<StudentCardDto>> GetStudentCard()
    {
        // Bước 1: Xác định người dùng đang thực hiện yêu cầu từ Token
        // Lấy mssv của người dùng đã đăng nhập từ claim trong JWT
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (loggedInMssv == null)
        {
            return Forbid();
        }

        // Bước 2: Truy vấn thông tin sinh viên từ database ===
        if (!int.TryParse(loggedInMssv, out int mssvInt))
        {
            return Forbid();
        }

        var student = await
            _context.Database.SqlQuery<CardInfoResult>(
            $"SELECT * FROM func_get_student_card_info({mssvInt})")
            .FirstOrDefaultAsync();

        if (student == null)
        {
            return NotFound(); // Không tìm thấy sinh viên với mssv này
        }

        // === Bước 3: Xây dựng đường dẫn URL đầy đủ cho ảnh thẻ ===
        string? avatarFullUrl = null;
        if (!string.IsNullOrEmpty(student.anh_the_url))
        {
            // Ghép địa chỉ server + request path + đường dẫn tương đối trong DB
            // Ví dụ: https://localhost:5093 + /files + /Students/Avatars/23520560.jpg
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            avatarFullUrl = $"{baseUrl}/files/{student.anh_the_url}";

        }

        // === Bước 4: Ánh xạ dữ liệu từ entity của database sang DTO để trả về ===
        var studentCard = new StudentCardDto
        {
            Mssv = student.mssv,
            HoTen = student.ho_ten,
            KhoaHoc = student.khoa_hoc,
            NganhHoc = student.nganh_hoc,
            AvatarFullUrl = avatarFullUrl
        };

        return Ok(studentCard);
    }

    /// <summary>
    /// Retrieves the quick GPA and accumulated credits for the currently authenticated student.
    /// </summary>
    [HttpGet("/quickgpa")]
    public async Task<ActionResult<QuickGpaDto>> GetQuickGpa()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (loggedInMssv == null) return Forbid();

        if (!int.TryParse(loggedInMssv, out int mssvInt))
        {
            return Forbid();
        }

        var result = await
            _context.Database.SqlQuery<QuickGpa>(
            $"SELECT * FROM func_calculate_gpa({mssvInt})")
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound(); // Không tìm thấy sinh viên với mssv này
        }

        var gpaAndCredits = new QuickGpaDto
        {
            Gpa = result.gpa,
            SoTinChiTichLuy = result.so_tin_chi_tich_luy
        };

        return Ok(gpaAndCredits);
    }

    // GET: api/students/academicresults
    [HttpGet("/academicresults")]
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
            HocKy = r.hoc_ky ?? string.Empty,
            MaMonHoc = r.ma_mon_hoc ?? string.Empty,
            TenMonHoc = r.ten_mon_hoc ?? string.Empty,
            SoTinChi = r.so_tin_chi ?? 0,
            TrongSoQuaTrinh = r.trong_so_qua_trinh ?? 0,
            TrongSoGiuaKi = r.trong_so_giua_ki ?? 0,
            TrongSoThucHanh = r.trong_so_thuc_hanh ?? 0,
            TrongSoCuoiKi = r.trong_so_cuoi_ki ?? 0,
            DiemQuaTrinh = r.diem_qua_trinh,
            DiemGiuaKi = r.diem_giua_ki,
            DiemThucHanh = r.diem_thuc_hanh,
            DiemCuoiKi = r.diem_cuoi_ki,
            DiemTongKet = r.diem_tong_ket
        }).ToList();

        return Ok(academicResults);
    }
}
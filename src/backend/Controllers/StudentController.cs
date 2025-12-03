using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eUIT.API.Data;
using eUIT.API.DTOs;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUIT.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly eUITDbContext _context;

    public StudentsController(eUITDbContext context)
    {
        _context = context;
    }

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

    private class QuickGpaResult
    {
        [Column("gpa")]
        public float Gpa { get; set; }
        [Column("so_tin_chi_tich_luy")]
        public int SoTinChiTichLuy { get; set; } = 0;
    }

    private class AcademicResultQueryResult
    {
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

    private class ScheduleQueryResult
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

    private class StudentProfileQueryResult
    {
        [Column("mssv")]
        public int Mssv { get; set; }
        [Column("ho_ten")]
        public string HoTen { get; set; } = string.Empty;
        [Column("ngay_sinh")]
        public DateOnly NgaySinh { get; set; }
        [Column("nganh_hoc")]
        public string NganhHoc { get; set; } = string.Empty;
        [Column("khoa_hoc")]
        public int KhoaHoc { get; set; }
        [Column("lop_sinh_hoat")]
        public string LopSinhHoat { get; set; } = string.Empty;

        [Column("noi_sinh")]
        public string? NoiSinh { get; set; }
        [Column("cccd")]
        public string? Cccd { get; set; }
        [Column("ngay_cap_cccd")]
        public DateOnly? NgayCapCccd { get; set; }
        [Column("noi_cap_cccd")]
        public string? NoiCapCccd { get; set; }
        [Column("dan_toc")]
        public string? DanToc { get; set; }
        [Column("ton_giao")]
        public string? TonGiao { get; set; }
        [Column("so_dien_thoai")]
        public string? SoDienThoai { get; set; }
        [Column("dia_chi_thuong_tru")]
        public string? DiaChiThuongTru { get; set; }
        [Column("tinh_thanh_pho")]
        public string? TinhThanhPho { get; set; }
        [Column("phuong_xa")]
        public string? PhuongXa { get; set; }
        [Column("qua_trinh_hoc_tap_cong_tac")]
        public string? QuaTrinhHocTapCongTac { get; set; }
        [Column("thanh_tich")]
        public string? ThanhTich { get; set; }
        [Column("email_ca_nhan")]
        public string? EmailCaNhan { get; set; }

        [Column("ma_ngan_hang")]
        public string? MaNganHang { get; set; }
        [Column("ten_ngan_hang")]
        public string? TenNganHang { get; set; }
        [Column("so_tai_khoan")]
        public string? SoTaiKhoan { get; set; }
        [Column("chi_nhanh")]
        public string? ChiNhanh { get; set; }

        [Column("ho_ten_cha")]
        public string? HoTenCha { get; set; }
        [Column("quoc_tich_cha")]
        public string? QuocTichCha { get; set; }
        [Column("dan_toc_cha")]
        public string? DanTocCha { get; set; }
        [Column("ton_giao_cha")]
        public string? TonGiaoCha { get; set; }
        [Column("sdt_cha")]
        public string? SdtCha { get; set; }
        [Column("email_cha")]
        public string? EmailCha { get; set; }
        [Column("dia_chi_thuong_tru_cha")]
        public string? DiaChiThuongTruCha { get; set; }
        [Column("cong_viec_cha")]
        public string? CongViecCha { get; set; }

        [Column("ho_ten_me")]
        public string? HoTenMe { get; set; }
        [Column("quoc_tich_me")]
        public string? QuocTichMe { get; set; }
        [Column("dan_toc_me")]
        public string? DanTocMe { get; set; }
        [Column("ton_giao_me")]
        public string? TonGiaoMe { get; set; }
        [Column("sdt_me")]
        public string? SdtMe { get; set; }
        [Column("email_me")]
        public string? EmailMe { get; set; }
        [Column("dia_chi_thuong_tru_me")]
        public string? DiaChiThuongTruMe { get; set; }
        [Column("cong_viec_me")]
        public string? CongViecMe { get; set; }

        [Column("ho_ten_ngh")]
        public string? HoTenNgh { get; set; }
        [Column("quoc_tich_ngh")]
        public string? QuocTichNgh { get; set; }
        [Column("dan_toc_ngh")]
        public string? DanTocNgh { get; set; }
        [Column("ton_giao_ngh")]
        public string? TonGiaoNgh { get; set; }
        [Column("sdt_ngh")]
        public string? SdtNgh { get; set; }
        [Column("email_ngh")]
        public string? EmailNgh { get; set; }
        [Column("dia_chi_thuong_tru_ngh")]
        public string? DiaChiThuongTruNgh { get; set; }
        [Column("cong_viec_ngh")]
        public string? CongViecNgh { get; set; }

        [Column("thong_tin_nguoi_can_bao_tin")]
        public string? ThongTinNguoiCanBaoTin { get; set; }
        [Column("so_dien_thoai_bao_tin")]
        public string? SoDienThoaiBaoTin { get; set; }

        [Column("anh_the_url")]
        public string? AnhTheUrl { get; set; }
    }

    private class RegisteredCourseQueryResult
{
    [Column("ma_lop")]
    public string MaLop { get; set; } = string.Empty;

    [Column("ma_mon_hoc")]
    public string MaMonHoc { get; set; } = string.Empty;

    [Column("ten_mon_hoc")]
    public string TenMonHoc { get; set; } = string.Empty;

    [Column("so_tin_chi")]
    public int SoTinChi { get; set; }

    [Column("ma_giang_vien")]
    public string MaGiangVien { get; set; } = string.Empty;
}

public class PrerequisiteDto
{
    [Column("ma_mon_hoc_dieu_kien")]
    public string MaMonHocDieuKien { get; set; } = string.Empty;

    [Column("ten_mon_hoc")]
    public string TenMonHoc { get; set; } = string.Empty;
}

public class ConductTotal
{

    [Column("mssv")]
    public int Mssv { get; set; }
    [Column("tong_diem_ren_luyen")]
    public decimal TongDiemRenLuyen { get; set; }
}

public class ConductDetail
{
    [Column("ma_hoat_dong")]
    public int MaHoatDong { get; set; }

    [Column("ten_hoat_dong")]
    public string TenHoatDong { get; set; } = string.Empty;

    [Column("ma_tieu_chi")]
    public string MaTieuChi { get; set; } = string.Empty;

    [Column("ten_tieu_chi")]
    public string TenTieuChi { get; set; } = string.Empty;

    [Column("he_so_tham_gia")]
    public int HeSoThamGia { get; set; }

    [Column("diem")]
    public int Diem { get; set; }

    [Column("tong_diem")]
    public int TongDiem { get; set; }

    [Column("ghi_chu")]
    public string? GhiChu { get; set; }
}

public class PersonalSchedule
{
    [Column("ngay")]
    public DateTime Ngay { get; set; }
    [Column("noi_dung")]
    public string NoiDung { get; set; } = string.Empty;
    [Column("ghi_chu")]
    public string? GhiChu { get; set; }
}


    //--- API Endpoints ---

    [HttpGet("nextclass")]
    public async Task<ActionResult<NextClassDto>> GetNextClass()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var nextClassResult = await _context.Database.SqlQuery<NextClassInfo>(
            $"SELECT * FROM func_get_next_class({mssvInt})")
            .FirstOrDefaultAsync();

        if (nextClassResult == null) return NoContent();

        var dto = new NextClassDto
        {
            MaLop = nextClassResult.MaLop,
            TenLop = nextClassResult.TenMonHocVn,
            Thu = nextClassResult.Thu,
            TietBatDau = nextClassResult.TietBatDau,
            TietKetThuc = nextClassResult.TietKetThuc,
            PhongHoc = nextClassResult.PhongHoc,
            NgayHoc = nextClassResult.NgayHoc,
            TenGiangVien = nextClassResult.TenGiangVien
        };

        return Ok(dto);
    }

    [HttpGet("card")]
    public async Task<ActionResult<StudentCardDto>> GetStudentCard()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var student = await _context.Database.SqlQuery<CardInfoResult>(
            $"SELECT * FROM func_get_student_card_info({mssvInt})")
            .FirstOrDefaultAsync();

        if (student == null) return NotFound();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var dto = new StudentCardDto
        {
            Mssv = student.Mssv,
            HoTen = student.HoTen,
            KhoaHoc = student.KhoaHoc,
            NganhHoc = student.NganhHoc,
            AvatarFullUrl = !string.IsNullOrEmpty(student.AnhTheUrl) 
                ? $"{baseUrl}/files/{student.AnhTheUrl}" 
                : null
        };

        return Ok(dto);
    }

    [HttpGet("quickgpa")]
    public async Task<ActionResult<QuickGpaDto>> GetQuickGpa()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var result = await _context.Database.SqlQuery<QuickGpaResult>(
            $"SELECT * FROM func_calculate_gpa({mssvInt})")
            .FirstOrDefaultAsync();

        if (result == null) return NotFound();

        return Ok(new QuickGpaDto
        {
            Gpa = result.Gpa,
            SoTinChiTichLuy = result.SoTinChiTichLuy
        });
    }

    [HttpGet("academicresults")]
    public async Task<ActionResult<IEnumerable<AcademicResultDTO>>> GetAcademicResults()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var queryResults = await _context.Database.SqlQuery<AcademicResultQueryResult>(
            $"SELECT * FROM chi_tiet_ket_qua_hoc_tap({mssvInt})")
            .ToListAsync();

        if (!queryResults.Any()) return NotFound();

        var dtos = queryResults.Select(r => new AcademicResultDTO
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

        return Ok(dtos);
    }

    [HttpGet("schedule/{hocKy}")]
    public async Task<ActionResult<IEnumerable<FullScheduleDto>>> GetFullSchedule(string hocKy)
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var queryResults = await _context.Database.SqlQuery<ScheduleQueryResult>(
            $"SELECT * FROM public.get_full_schedule({mssvInt}, {hocKy})")
            .ToListAsync();

        if (!queryResults.Any()) return NoContent();

        var dtos = queryResults.Select(r => new FullScheduleDto
        {
            MaLop = r.MaLop,
            TenLop = r.TenMonHocVn,
            Thu = r.Thu,
            TietBatDau = r.TietBatDau,
            TietKetThuc = r.TietKetThuc,
            PhongHoc = r.PhongHoc,
            NgayHoc = r.NgayHoc,
            TenGiangVien = r.TenGiangVien
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("profile")]
    public async Task<ActionResult<StudentProfileDto>> GetStudentProfile()
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var result = await _context.Database.SqlQuery<StudentProfileQueryResult>(
            $"SELECT * FROM func_get_student_profile({mssvInt})")
            .FirstOrDefaultAsync();

        if (result == null) return NotFound();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var dto = new StudentProfileDto
        {
            Mssv = result.Mssv,
            HoTen = result.HoTen,
            NgaySinh = result.NgaySinh,
            NganhHoc = result.NganhHoc,
            KhoaHoc = result.KhoaHoc,
            LopSinhHoat = result.LopSinhHoat,
            NoiSinh = result.NoiSinh,
            Cccd = result.Cccd,
            NgayCapCccd = result.NgayCapCccd,
            NoiCapCccd = result.NoiCapCccd,
            DanToc = result.DanToc,
            TonGiao = result.TonGiao,
            SoDienThoai = result.SoDienThoai,
            DiaChiThuongTru = result.DiaChiThuongTru,
            TinhThanhPho = result.TinhThanhPho,
            PhuongXa = result.PhuongXa,
            QuaTrinhHocTapCongTac = result.QuaTrinhHocTapCongTac,
            ThanhTich = result.ThanhTich,
            EmailCaNhan = result.EmailCaNhan,
            MaNganHang = result.MaNganHang,
            TenNganHang = result.TenNganHang,
            SoTaiKhoan = result.SoTaiKhoan,
            ChiNhanh = result.ChiNhanh,
            Cha = new ThongTinPhuHuynh
            {
                HoTen = result.HoTenCha,
                QuocTich = result.QuocTichCha,
                DanToc = result.DanTocCha,
                TonGiao = result.TonGiaoCha,
                SoDienThoai = result.SdtCha,
                Email = result.EmailCha,
                DiaChiThuongTru = result.DiaChiThuongTruCha,
                CongViec = result.CongViecCha
            },
            Me = new ThongTinPhuHuynh
            {
                HoTen = result.HoTenMe,
                QuocTich = result.QuocTichMe,
                DanToc = result.DanTocMe,
                TonGiao = result.TonGiaoMe,
                SoDienThoai = result.SdtMe,
                Email = result.EmailMe,
                DiaChiThuongTru = result.DiaChiThuongTruMe,
                CongViec = result.CongViecMe
            },
            NguoiGiamHo = new ThongTinPhuHuynh
            {
                HoTen = result.HoTenNgh,
                QuocTich = result.QuocTichNgh,
                DanToc = result.DanTocNgh,
                TonGiao = result.TonGiaoNgh,
                SoDienThoai = result.SdtNgh,
                Email = result.EmailNgh,
                DiaChiThuongTru = result.DiaChiThuongTruNgh,
                CongViec = result.CongViecNgh
            },
            ThongTinNguoiCanBaoTin = result.ThongTinNguoiCanBaoTin,
            SoDienThoaiBaoTin = result.SoDienThoaiBaoTin,
            AvatarFullUrl = !string.IsNullOrEmpty(result.AnhTheUrl) 
                ? $"{baseUrl}/files/{result.AnhTheUrl}" 
                : null
        };

        return Ok(dto);
    }
// PUT: /api/students/avatar
    [HttpPut("avatar")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AvatarUploadDto>> UpdateStudentAvatar([FromForm] UpdateAvatarDto request)
    {
        var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

        var file = request.AvatarFile;
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type. Only jpg, jpeg, png allowed.");

        var student = await _context.Students.FirstOrDefaultAsync(s => s.Mssv == mssvInt);
        if (student == null) return NotFound();

        if (!string.IsNullOrEmpty(student.AnhTheUrl))
        {
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", student.AnhTheUrl);
            if (System.IO.File.Exists(oldFilePath))
                System.IO.File.Delete(oldFilePath);
        }

        var newFileName = $"avatar_{mssvInt}_{DateTime.Now.Ticks}{extension}";
        var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", newFileName);

        Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

        await using (var stream = new FileStream(savePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        student.AnhTheUrl = newFileName;
        await _context.SaveChangesAsync();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var dto = new AvatarUploadDto
        {
            AvatarFullUrl = $"{baseUrl}/files/{newFileName}"
        };

        return Ok(dto);
    }
[HttpGet("exams")]
public async Task<ActionResult<IEnumerable<StudentExamDto>>> GetExamSchedule(
    [FromQuery] string hocKy)
{
    // Lấy MSSV từ JWT
    var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(loggedInMssv, out int mssvInt))
        return Forbid();

    if (string.IsNullOrWhiteSpace(hocKy))
        return BadRequest("Vui lòng truyền `hocKy` dạng '2025_2026_1'");

    // Gọi function Postgres
    var exams = await _context.Set<StudentExamDto>()
        .FromSqlInterpolated(
            $@"SELECT * FROM func_get_exam_schedule({mssvInt}, {hocKy})"
        )
        .ToListAsync();

    if (!exams.Any())
        return NoContent();

    return Ok(exams);
}


[HttpGet("registered-courses")]
public async Task<ActionResult<IEnumerable<RegisteredCourseDto>>> GetRegisteredCourses()
{
    var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

    var coursesQuery = await _context.Database
        .SqlQuery<RegisteredCourseQueryResult>($"SELECT * FROM func_get_student_registered_courses({mssvInt})")
        .ToListAsync();

    if (!coursesQuery.Any()) return NoContent();

    // Map sang DTO public nếu muốn tách lớp private/public
    var courses = coursesQuery.Select(c => new RegisteredCourseDto
    {
        MaLop = c.MaLop,
        MaMonHoc = c.MaMonHoc,
        TenMonHoc = c.TenMonHoc,
        SoTinChi = c.SoTinChi,
        MaGiangVien = c.MaGiangVien
    }).ToList();

    return Ok(courses);
}

[HttpGet("prerequisites")]
public async Task<ActionResult<IEnumerable<PrerequisiteDto>>> GetPrerequisites([FromQuery] string maMon)
{
    var prerequisites = await _context.Database
        .SqlQuery<PrerequisiteDto>($"SELECT * FROM func_get_prerequisites({maMon})")
        .ToListAsync();

    if (!prerequisites.Any()) return NoContent();

    return Ok(prerequisites);
}

// GET: /api/Students/conduct/total
[HttpGet("conduct/total")]
public async Task<ActionResult<ConductTotal>> GetTotalConduct()
{
    var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

    var totalResult = await _context.Database
        .SqlQuery<ConductTotal>($"SELECT * FROM func_total_conduct({mssvInt})")
        .FirstOrDefaultAsync();

    if (totalResult == null)
    {
        totalResult = new ConductTotal
        {
            Mssv = mssvInt,
            TongDiemRenLuyen = 0
        };
    }

    return Ok(totalResult);
}



// GET: /api/Students/conduct/details
[HttpGet("conduct/details")]
public async Task<ActionResult<IEnumerable<ConductDetail>>> GetConductDetails()
{
    var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

    var conductList = await _context.Database
    .SqlQuery<ConductDetail>($"SELECT * FROM func_conduct_list({mssvInt})")
    .ToListAsync();


    if (!conductList.Any()) return NoContent();

    return Ok(conductList);
}

[HttpGet("personal-schedule")]
public async Task<ActionResult<IEnumerable<PersonalSchedule>>> GetPersonalSchedule()
{
    var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

    var result = new List<PersonalSchedule>();

    using (var conn = _context.Database.GetDbConnection())
    {
        await conn.OpenAsync();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT * FROM func_get_personal_schedule(@mssv)";
            var param = cmd.CreateParameter();
            param.ParameterName = "@mssv";
            param.Value = mssvInt;
            cmd.Parameters.Add(param);

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.Add(new PersonalSchedule
                    {
                        Ngay = reader.GetDateTime(reader.GetOrdinal("ngay")),
                        NoiDung = reader.GetString(reader.GetOrdinal("noi_dung")),
                        GhiChu = reader.IsDBNull(reader.GetOrdinal("ghi_chu")) 
                            ? null 
                            : reader.GetString(reader.GetOrdinal("ghi_chu"))
                    });
                }
            }
        }
    }

    if (!result.Any()) return NoContent();
    return Ok(result);
}



[HttpPut("personal-schedule")]
public async Task<IActionResult> UpdatePersonalSchedule([FromBody] PersonalSchedule model)
{
    var loggedInMssv = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (!int.TryParse(loggedInMssv, out int mssvInt)) return Forbid();

    await _context.Database.ExecuteSqlInterpolatedAsync($@"
        INSERT INTO lich_ca_nhan (mssv, ngay, noi_dung, ghi_chu)
        VALUES ({mssvInt}, {model.Ngay}, {model.NoiDung}, {model.GhiChu})
        ON CONFLICT (mssv, ngay)
        DO UPDATE SET 
            noi_dung = EXCLUDED.noi_dung,
            ghi_chu = EXCLUDED.ghi_chu;
    ");

    return Ok(new { message = "Cập nhật lịch cá nhân thành công" });
}


}

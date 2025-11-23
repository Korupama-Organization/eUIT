using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using eUIT.API.DTOs;
using eUIT.API.DTOs.Create;
using eUIT.API.Data;

namespace eUIT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly eUITDbContext _context;

        public ServicesController(eUITDbContext context)
        {
            _context = context;
        }

        private int GetMssvFromToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim ?? "0");
        }

        // ============================================
        // 1) Lấy danh sách giấy xác nhận sinh viên
        // ============================================
        [HttpGet("student-confirmation")]
        public async Task<IActionResult> GetStudentConfirmation()
        {
            int mssv = GetMssvFromToken();

            string query = @"
                SELECT 
                    id AS ""Id"", 
                    mssv AS ""Mssv"", 
                    ngon_ngu AS ""NgonNgu"", 
                    ly_do AS ""LyDo"", 
                    ly_do_khac AS ""LyDoKhac"", 
                    ngay_dang_ky AS ""NgayDangKy"", 
                    trang_thai AS ""TrangThai"", 
                    ghi_chu AS ""GhiChu""
                FROM gxn_sinh_vien
                WHERE mssv = {0}
                ORDER BY ngay_dang_ky DESC";

            var data = await _context.Database
                .SqlQueryRaw<StudentConfirmationDto>(query, mssv)
                .ToListAsync();

            return Ok(data);
        }

        // ============================================
        // 2) Đăng ký giấy xác nhận sinh viên
        // ============================================
        [HttpPost("student-confirmation")]
        public async Task<IActionResult> CreateStudentConfirmation(StudentConfirmationCreateDto dto)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    INSERT INTO gxn_sinh_vien
                    (mssv, ngon_ngu, ly_do, ly_do_khac, ngay_dang_ky, trang_thai)
                    VALUES ({0}, {1}, {2}, {3}, NOW(), 'CHO_DUYET')";

                await _context.Database.ExecuteSqlRawAsync(sql,
                    mssv, dto.NgonNgu, dto.LyDo, dto.LyDoKhac ?? "");

                return Ok(new { message = "Đăng ký giấy xác nhận thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng ký!" });
            }
        }

        // ============================================
        // 3) Lịch sử phúc khảo
        // ============================================
        [HttpGet("re-exam")]
        public async Task<IActionResult> GetReExam()
        {
            int mssv = GetMssvFromToken();

            string sql = @"
                SELECT 
                    p.mssv AS ""Mssv"", 
                    p.ma_mon_hoc AS ""MaMon"", 
                    p.ma_lop AS ""MaLop"",
                    0 AS ""LanThi"",
                    COALESCE(l.ngay_thi, p.ngay_dang_ky) AS ""NgayThi"", 
                    COALESCE(l.phong_thi, '') AS ""PhongThi"", 
                    COALESCE(l.ca_thi, 0) AS ""CaThi"", 
                    p.trang_thai AS ""TrangThai"",
                    p.ngay_dang_ky AS ""NgayDangKy""
                FROM phuc_khao p
                LEFT JOIN lich_thi l ON p.ma_lop = l.ma_lop
                WHERE p.mssv = {0}
                ORDER BY p.ngay_dang_ky DESC";

            var data = await _context.Database
                .SqlQueryRaw<ReExamDto>(sql, mssv)
                .ToListAsync();

            return Ok(data);
        }

        // ============================================
        // 4) Đăng ký phúc khảo
        // ============================================
        [HttpPost("re-exam")]
        public async Task<IActionResult> CreateReExam(ReExamCreateDto dto)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    INSERT INTO phuc_khao
                    (mssv, ma_mon_hoc, ma_lop, ngay_dang_ky, trang_thai, ly_do)
                    VALUES ({0}, {1}, {2}, NOW(), 'CHO_DUYET', '')";

                await _context.Database.ExecuteSqlRawAsync(sql,
                    mssv, dto.MaMonHoc, dto.MaLop);

                return Ok(new { message = "Đăng ký phúc khảo thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng ký!" });
            }
        }

        // ============================================
        // 5) Lịch sử gửi xe
        // ============================================
        [HttpGet("parking")]
        public async Task<IActionResult> GetParking()
        {
            int mssv = GetMssvFromToken();

            string sql = @"
                SELECT 
                    mssv AS ""Mssv"", 
                    ma_bien_so AS ""MaBienSo"", 
                    so_thang AS ""SoThang"", 
                    so_tien AS ""SoTien"",
                    tinh_trang AS ""TinhTrang"", 
                    ngay_dang_ky AS ""NgayDangKy"",
                    ngay_het_han AS ""NgayHetHan""
                FROM dang_ky_gui_xe
                WHERE mssv = {0}
                ORDER BY ngay_dang_ky DESC";

            var data = await _context.Database
                .SqlQueryRaw<ParkingRegistrationDto>(sql, mssv)
                .ToListAsync();

            return Ok(data);
        }

        // ============================================
        // 6) Đăng ký gửi xe
        // ============================================
        [HttpPost("parking")]
        public async Task<IActionResult> CreateParking(ParkingRegistrationCreateDto dto)
        {
            int mssv = GetMssvFromToken();

            try
            {
                decimal soTien = dto.SoThang * 40000;

                string sql = @"
                    INSERT INTO dang_ky_gui_xe
                    (mssv, ma_bien_so, so_thang, so_tien, tinh_trang, 
                     ngay_dang_ky, ngay_thanh_toan, ngay_het_han)
                    VALUES ({0}, {1}, {2}, {3}, 'CHO_THANH_TOAN',
                            NOW(), NULL, NOW() + CAST({2} || ' months' AS INTERVAL))";

                await _context.Database.ExecuteSqlRawAsync(sql,
                    mssv, dto.MaBienSo, dto.SoThang, soTien);

                return Ok(new { message = "Đăng ký gửi xe thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng ký!" });
            }
        }

        // ============================================
        // 7) Lịch sử bảng điểm
        // ============================================
        [HttpGet("transcript")]
        public async Task<IActionResult> GetTranscript()
        {
            int mssv = GetMssvFromToken();

            string sql = @"
                SELECT 
                    mssv AS ""Mssv"", 
                    loai_bang_diem AS ""LoaiBangDiem"", 
                    ngon_ngu AS ""NgonNgu"", 
                    so_luong AS ""SoLuong"",
                    ngay_dang_ky AS ""NgayDangKy"", 
                    trang_thai AS ""TrangThai"", 
                    CAST(COALESCE(chi_phi, 0) AS TEXT) AS ""ChiPhi""
                FROM bang_diem_yeu_cau
                WHERE mssv = {0}
                ORDER BY ngay_dang_ky DESC";

            var data = await _context.Database
                .SqlQueryRaw<TranscriptRequestDto>(sql, mssv)
                .ToListAsync();

            return Ok(data);
        }

        // ============================================
        // 8) Đăng ký bảng điểm
        // ============================================
        [HttpPost("transcript")]
        public async Task<IActionResult> CreateTranscript(TranscriptRequestCreateDto dto)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    INSERT INTO bang_diem_yeu_cau
                    (mssv, loai_bang_diem, ngon_ngu, so_luong,
                     ngay_dang_ky, trang_thai, chi_phi)
                    VALUES ({0}, {1}, {2}, {3}, NOW(), 'CHO_DUYET', 0)";

                await _context.Database.ExecuteSqlRawAsync(sql,
                    mssv, dto.LoaiBangDiem, dto.NgonNgu, dto.SoLuong);

                return Ok(new { message = "Đăng ký bảng điểm thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng ký!" });
            }
        }

        // ============================================
        // 9) Lịch sử giấy giới thiệu
        // ============================================
        [HttpGet("introduction")]
        public async Task<IActionResult> GetIntroduction()
        {
            int mssv = GetMssvFromToken();

            string sql = @"
                SELECT 
                    mssv AS ""Mssv"", 
                    '' AS ""NoiNhan"",
                    muc_dich AS ""MucDich"", 
                    ngay_dang_ky AS ""NgayDangKy"", 
                    trang_thai AS ""TrangThai""
                FROM giay_gioi_thieu
                WHERE mssv = {0}
                ORDER BY ngay_dang_ky DESC";

            var data = await _context.Database
                .SqlQueryRaw<IntroductionLetterDto>(sql, mssv)
                .ToListAsync();

            return Ok(data);
        }

        // ============================================
        // 10) Đăng ký giấy giới thiệu
        // ============================================
        [HttpPost("introduction")]
        public async Task<IActionResult> CreateIntroduction(IntroductionLetterCreateDto dto)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    INSERT INTO giay_gioi_thieu
                    (mssv, muc_dich, ngay_dang_ky, trang_thai)
                    VALUES ({0}, {1}, NOW(), 'CHO_DUYET')";

                await _context.Database.ExecuteSqlRawAsync(sql,
                    mssv, dto.MucDich);

                return Ok(new { message = "Đăng ký giấy giới thiệu thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng ký!" });
            }
        }

        // ============================================
        // 11) Lịch sử xác nhận chứng chỉ
        // ============================================
        [HttpGet("certificate-confirmation")]
        public async Task<IActionResult> GetCertificateConfirmation()
        {
            int mssv = GetMssvFromToken();

            string sql = @"
                SELECT 
                    mssv AS ""Mssv"", 
                    ma_chung_chi AS ""MaChungChi"", 
                    loai_chung_chi AS ""LoaiChungChi"", 
                    ngay_thi AS ""NgayThi"",
                    tinh_trang AS ""TinhTrang"", 
                    ngay_dang_ky AS ""NgayDangKy"",
                    ghi_chu AS ""GhiChu""
                FROM xac_nhan_chung_chi
                WHERE mssv = {0}
                ORDER BY ngay_dang_ky DESC";

            var data = await _context.Database
                .SqlQueryRaw<CertificateConfirmationDto>(sql, mssv)
                .ToListAsync();

            return Ok(data);
        }

        // ============================================
        // 12) Đăng ký xác nhận chứng chỉ
        // ============================================
        [HttpPost("certificate-confirmation")]
        public async Task<IActionResult> CreateCertificateConfirmation(CertificateConfirmationCreateDto dto)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    INSERT INTO xac_nhan_chung_chi
                    (mssv, ma_chung_chi, loai_chung_chi, ngay_thi, 
                     tinh_trang, ngay_dang_ky, ghi_chu)
                    VALUES ({0}, {1}, {2}, {3}, 'CHO_DUYET', NOW(), '')";

                await _context.Database.ExecuteSqlRawAsync(sql,
                    mssv, dto.MaChungChi, dto.LoaiChungChi, dto.NgayThi);

                return Ok(new { message = "Đăng ký xác nhận chứng chỉ thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi đăng ký!" });
            }
        }

        // ============================================
        // DELETE ENDPOINTS - Hủy/Xóa dịch vụ
        // ============================================

        // 13) Hủy giấy xác nhận sinh viên
        [HttpDelete("student-confirmation/{id}")]
        public async Task<IActionResult> DeleteStudentConfirmation(int id)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    UPDATE gxn_sinh_vien 
                    SET trang_thai = 'DA_HUY'
                    WHERE id = {0} AND mssv = {1} AND trang_thai = 'CHO_DUYET'";

                var result = await _context.Database.ExecuteSqlRawAsync(sql, id, mssv);

                if (result == 0)
                {
                    return NotFound(new { message = "Không tìm thấy yêu cầu hoặc không thể hủy!" });
                }

                return Ok(new { message = "Hủy giấy xác nhận thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi hủy!" });
            }
        }

        // 14) Hủy phúc khảo
        [HttpDelete("re-exam/{id}")]
        public async Task<IActionResult> DeleteReExam(int id)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    UPDATE phuc_khao 
                    SET trang_thai = 'DA_HUY'
                    WHERE id = {0} AND mssv = {1} AND trang_thai = 'CHO_DUYET'";

                var result = await _context.Database.ExecuteSqlRawAsync(sql, id, mssv);

                if (result == 0)
                {
                    return NotFound(new { message = "Không tìm thấy yêu cầu hoặc không thể hủy!" });
                }

                return Ok(new { message = "Hủy phúc khảo thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi hủy!" });
            }
        }

        // 15) Hủy đăng ký gửi xe
        [HttpDelete("parking/{mssv}/{maBienSo}")]
        public async Task<IActionResult> DeleteParking(int mssv, string maBienSo)
        {
            int mssvToken = GetMssvFromToken();

            if (mssv != mssvToken)
            {
                return Forbid();
            }

            try
            {
                string sql = @"
                    UPDATE dang_ky_gui_xe 
                    SET tinh_trang = 'DA_HUY'
                    WHERE mssv = {0} AND ma_bien_so = {1} AND tinh_trang = 'CHO_THANH_TOAN'";

                var result = await _context.Database.ExecuteSqlRawAsync(sql, mssv, maBienSo);

                if (result == 0)
                {
                    return NotFound(new { message = "Không tìm thấy đăng ký hoặc không thể hủy!" });
                }

                return Ok(new { message = "Hủy đăng ký gửi xe thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi hủy!" });
            }
        }

        // 16) Hủy yêu cầu bảng điểm
        [HttpDelete("transcript/{id}")]
        public async Task<IActionResult> DeleteTranscript(int id)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    UPDATE bang_diem_yeu_cau 
                    SET trang_thai = 'DA_HUY'
                    WHERE id = {0} AND mssv = {1} AND trang_thai = 'CHO_DUYET'";

                var result = await _context.Database.ExecuteSqlRawAsync(sql, id, mssv);

                if (result == 0)
                {
                    return NotFound(new { message = "Không tìm thấy yêu cầu hoặc không thể hủy!" });
                }

                return Ok(new { message = "Hủy yêu cầu bảng điểm thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi hủy!" });
            }
        }

        // 17) Hủy giấy giới thiệu
        [HttpDelete("introduction/{id}")]
        public async Task<IActionResult> DeleteIntroduction(int id)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    UPDATE giay_gioi_thieu 
                    SET trang_thai = 'DA_HUY'
                    WHERE id = {0} AND mssv = {1} AND trang_thai = 'CHO_DUYET'";

                var result = await _context.Database.ExecuteSqlRawAsync(sql, id, mssv);

                if (result == 0)
                {
                    return NotFound(new { message = "Không tìm thấy yêu cầu hoặc không thể hủy!" });
                }

                return Ok(new { message = "Hủy giấy giới thiệu thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi hủy!" });
            }
        }

        // 18) Hủy xác nhận chứng chỉ
        [HttpDelete("certificate-confirmation/{maChungChi}")]
        public async Task<IActionResult> DeleteCertificateConfirmation(string maChungChi)
        {
            int mssv = GetMssvFromToken();

            try
            {
                string sql = @"
                    UPDATE xac_nhan_chung_chi 
                    SET tinh_trang = 'DA_HUY'
                    WHERE mssv = {0} AND ma_chung_chi = {1} AND tinh_trang = 'CHO_DUYET'";

                var result = await _context.Database.ExecuteSqlRawAsync(sql, mssv, maChungChi);

                if (result == 0)
                {
                    return NotFound(new { message = "Không tìm thấy yêu cầu hoặc không thể hủy!" });
                }

                return Ok(new { message = "Hủy xác nhận chứng chỉ thành công!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi hủy!" });
            }
        }
    }
}
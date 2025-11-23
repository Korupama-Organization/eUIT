using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using eUIT.API.Data;
using eUIT.API.DTOs;
using eUIT.API.DTOs.Create;

namespace eUIT.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly eUITDbContext _context;

        public SettingsController(eUITDbContext context)
        {
            _context = context;
        }

        private int GetMssvFromToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(claim ?? "0");
        }

        // ============================================
        // 1) LẤY CÀI ĐẶT NGƯỜI DÙNG
        // ============================================
        [HttpGet("user-settings")]
        public async Task<IActionResult> GetUserSettings()
        {
            int mssv = GetMssvFromToken();

            string sql = @"
                SELECT 
                    mssv AS ""Mssv"",
                    che_do_toi AS ""CheDoToi"",
                    cap_nhat_ket_qua_hoc_tap AS ""CapNhatKetQuaHocTap"",
                    thong_bao_nghi_lop AS ""ThongBaoNghiLop"",
                    thong_bao_hoc_bu AS ""ThongBaoHocBu"",
                    lich_thi AS ""LichThi"",
                    thong_bao_moi AS ""ThongBaoMoi"",
                    cap_nhat_trang_thai_thu_tuc_hanh_chinh AS ""CapNhatTrangThaiThuTucHanhChinh"",
                    bat_thong_bao_email AS ""BatThongBaoEmail"",
                    ngay_tao AS ""NgayTao"",
                    ngay_cap_nhat AS ""NgayCapNhat""
                FROM cai_dat_nguoi_dung
                WHERE mssv = {0}";

            var settings = await _context.Database
                .SqlQueryRaw<UserSettingsDto>(sql, mssv)
                .FirstOrDefaultAsync();

            if (settings == null)
            {
                // Tạo cài đặt mặc định nếu chưa có
                await _context.Database.ExecuteSqlRawAsync(@"
                    INSERT INTO cai_dat_nguoi_dung (mssv) VALUES ({0})", mssv);

                settings = await _context.Database
                    .SqlQueryRaw<UserSettingsDto>(sql, mssv)
                    .FirstOrDefaultAsync();
            }

            return Ok(settings);
        }

        // ============================================
        // 2) CẬP NHẬT CÀI ĐẶT NGƯỜI DÙNG
        // ============================================
        [HttpPut("user-settings")]
        public async Task<IActionResult> UpdateUserSettings(UpdateUserSettingsDto dto)
        {
            int mssv = GetMssvFromToken();

            try
            {
                var updates = new List<string>();
                var parameters = new List<object> { mssv };
                int paramIndex = 1;

                if (dto.CheDoToi.HasValue)
                {
                    updates.Add($"che_do_toi = {{{paramIndex++}}}");
                    parameters.Add(dto.CheDoToi.Value);
                }
                if (dto.CapNhatKetQuaHocTap.HasValue)
                {
                    updates.Add($"cap_nhat_ket_qua_hoc_tap = {{{paramIndex++}}}");
                    parameters.Add(dto.CapNhatKetQuaHocTap.Value);
                }
                if (dto.ThongBaoNghiLop.HasValue)
                {
                    updates.Add($"thong_bao_nghi_lop = {{{paramIndex++}}}");
                    parameters.Add(dto.ThongBaoNghiLop.Value);
                }
                if (dto.ThongBaoHocBu.HasValue)
                {
                    updates.Add($"thong_bao_hoc_bu = {{{paramIndex++}}}");
                    parameters.Add(dto.ThongBaoHocBu.Value);
                }
                if (dto.LichThi.HasValue)
                {
                    updates.Add($"lich_thi = {{{paramIndex++}}}");
                    parameters.Add(dto.LichThi.Value);
                }
                if (dto.ThongBaoMoi.HasValue)
                {
                    updates.Add($"thong_bao_moi = {{{paramIndex++}}}");
                    parameters.Add(dto.ThongBaoMoi.Value);
                }
                if (dto.CapNhatTrangThaiThuTucHanhChinh.HasValue)
                {
                    updates.Add($"cap_nhat_trang_thai_thu_tuc_hanh_chinh = {{{paramIndex++}}}");
                    parameters.Add(dto.CapNhatTrangThaiThuTucHanhChinh.Value);
                }
                if (dto.BatThongBaoEmail.HasValue)
                {
                    updates.Add($"bat_thong_bao_email = {{{paramIndex++}}}");
                    parameters.Add(dto.BatThongBaoEmail.Value);
                }

                if (updates.Count == 0)
                {
                    return BadRequest(new { message = "Không có thông tin nào để cập nhật!" });
                }

                string sql = $@"
                    UPDATE cai_dat_nguoi_dung
                    SET {string.Join(", ", updates)},
                        ngay_cap_nhat = NOW()
                    WHERE mssv = {{0}}";

                await _context.Database.ExecuteSqlRawAsync(sql, parameters.ToArray());

                return Ok(new { message = "Cập nhật cài đặt thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật!", error = ex.Message });
            }
        }
    }
}
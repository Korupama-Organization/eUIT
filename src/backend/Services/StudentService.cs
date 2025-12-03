using Microsoft.EntityFrameworkCore;
using eUIT.API.Data;
using eUIT.API.DTOs;
using eUIT.API.Models;

namespace eUIT.API.Services;

/// <summary>
/// Service xử lý nghiệp vụ tra cứu sinh viên
/// </summary>
public class StudentService : IStudentService
{
    private readonly eUITDbContext _context;

    public StudentService(eUITDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Tìm kiếm sinh viên với nhiều tiêu chí và phân trang
    /// </summary>
    public async Task<(IEnumerable<StudentSearchDTO> students, int totalCount)> SearchStudentsAsync(StudentSearchRequestDTO searchRequest)
    {
        // Bắt đầu với query cơ bản
        var query = _context.sinh_vien.AsQueryable();

        // Tìm kiếm theo Keyword (MSSV hoặc Họ tên)
        if (!string.IsNullOrWhiteSpace(searchRequest.Keyword))
        {
            var keyword = searchRequest.Keyword.Trim().ToLower();
            
            // Thử parse keyword thành số để tìm MSSV
            if (int.TryParse(keyword, out int mssvSearch))
            {
                query = query.Where(s => s.Mssv == mssvSearch);
            }
            else
            {
                // Tìm theo tên (không phân biệt hoa thường)
                query = query.Where(s => s.HoTen.ToLower().Contains(keyword));
            }
        }

        // Tìm theo MSSV cụ thể
        if (searchRequest.Mssv.HasValue)
        {
            query = query.Where(s => s.Mssv == searchRequest.Mssv.Value);
        }

        // Tìm theo Họ tên (gần đúng)
        if (!string.IsNullOrWhiteSpace(searchRequest.HoTen))
        {
            var hoTen = searchRequest.HoTen.Trim().ToLower();
            query = query.Where(s => s.HoTen.ToLower().Contains(hoTen));
        }

        // Lọc theo Lớp sinh hoạt
        if (!string.IsNullOrWhiteSpace(searchRequest.LopSinhHoat))
        {
            var lop = searchRequest.LopSinhHoat.Trim().ToLower();
            query = query.Where(s => s.LopSinhHoat.ToLower().Contains(lop));
        }

        // Lọc theo Ngành học
        if (!string.IsNullOrWhiteSpace(searchRequest.NganhHoc))
        {
            var nganh = searchRequest.NganhHoc.Trim().ToLower();
            query = query.Where(s => s.NganhHoc.ToLower().Contains(nganh));
        }

        // Lọc theo Khóa học
        if (searchRequest.KhoaHoc.HasValue)
        {
            query = query.Where(s => s.KhoaHoc == searchRequest.KhoaHoc.Value);
        }

        // Đếm tổng số kết quả (trước khi phân trang)
        var totalCount = await query.CountAsync();

        // Áp dụng phân trang
        var pageSize = searchRequest.PageSize > 0 ? searchRequest.PageSize : 20;
        var pageNumber = searchRequest.PageNumber > 0 ? searchRequest.PageNumber : 1;
        var skip = (pageNumber - 1) * pageSize;

        // Lấy dữ liệu với phân trang, sắp xếp theo MSSV
        var students = await query
            .OrderBy(s => s.Mssv)
            .Skip(skip)
            .Take(pageSize)
            .Select(s => new StudentSearchDTO
            {
                Mssv = s.Mssv,
                HoTen = s.HoTen ?? string.Empty,
                LopSinhHoat = s.LopSinhHoat ?? string.Empty,
                NganhHoc = s.NganhHoc ?? string.Empty,
                KhoaHoc = s.KhoaHoc,
                SoDienThoai = s.SoDienThoai ?? string.Empty,
                EmailCaNhan = s.EmailCaNhan ?? string.Empty,
                AnhTheUrl = s.AnhTheUrl ?? string.Empty
            })
            .ToListAsync();

        return (students, totalCount);
    }

    /// <summary>
    /// Lấy thông tin chi tiết sinh viên theo MSSV
    /// </summary>
    public async Task<StudentDTO?> GetStudentByIdAsync(int studentId)
    {
        var student = await _context.sinh_vien.FindAsync(studentId);
        
        if (student == null)
            return null;

        return MapToDTO(student);
    }

    /// <summary>
    /// Helper method để map từ Model sang DTO đầy đủ
    /// </summary>
    private static StudentDTO MapToDTO(Student student)
    {
        return new StudentDTO
        {
            // Thông tin cơ bản
            Mssv = student.Mssv,
            HoTen = student.HoTen ?? string.Empty,
            NgaySinh = student.NgaySinh,
            NganhHoc = student.NganhHoc ?? string.Empty,
            KhoaHoc = student.KhoaHoc,
            LopSinhHoat = student.LopSinhHoat ?? string.Empty,
            NoiSinh = student.NoiSinh ?? string.Empty,
            
            // Giấy tờ tùy thân
            Cccd = student.Cccd ?? string.Empty,
            NgayCapCccd = student.NgayCapCccd,
            NoiCapCccd = student.NoiCapCccd ?? string.Empty,
            
            // Thông tin cá nhân
            DanToc = student.DanToc ?? string.Empty,
            TonGiao = student.TonGiao ?? string.Empty,
            SoDienThoai = student.SoDienThoai ?? string.Empty,
            DiaChiThuongTru = student.DiaChiThuongTru ?? string.Empty,
            TinhThanhPho = student.TinhThanhPho ?? string.Empty,
            PhuongXa = student.PhuongXa ?? string.Empty,
            
            // Thông tin học tập
            QuaTrinhHocTapCongTac = student.QuaTrinhHocTapCongTac ?? string.Empty,
            ThanhTich = student.ThanhTich ?? string.Empty,
            EmailCaNhan = student.EmailCaNhan ?? string.Empty,
            
            // Thông tin ngân hàng
            MaNganHang = student.MaNganHang ?? string.Empty,
            TenNganHang = student.TenNganHang ?? string.Empty,
            SoTaiKhoan = student.SoTaiKhoan ?? string.Empty,
            ChiNhanh = student.ChiNhanh ?? string.Empty,
            
            // Thông tin cha
            HoTenCha = student.HoTenCha ?? string.Empty,
            QuocTichCha = student.QuocTichCha ?? string.Empty,
            DanTocCha = student.DanTocCha ?? string.Empty,
            TonGiaoCha = student.TonGiaoCha ?? string.Empty,
            SdtCha = student.SdtCha ?? string.Empty,
            EmailCha = student.EmailCha ?? string.Empty,
            DiaChiThuongTruCha = student.DiaChiThuongTruCha ?? string.Empty,
            CongViecCha = student.CongViecCha ?? string.Empty,
            
            // Thông tin mẹ
            HoTenMe = student.HoTenMe ?? string.Empty,
            QuocTichMe = student.QuocTichMe ?? string.Empty,
            DanTocMe = student.DanTocMe ?? string.Empty,
            TonGiaoMe = student.TonGiaoMe ?? string.Empty,
            SdtMe = student.SdtMe ?? string.Empty,
            EmailMe = student.EmailMe ?? string.Empty,
            DiaChiThuongTruMe = student.DiaChiThuongTruMe ?? string.Empty,
            CongViecMe = student.CongViecMe ?? string.Empty,
            
            // Thông tin người giám hộ
            HoTenNgh = student.HoTenNgh ?? string.Empty,
            QuocTichNgh = student.QuocTichNgh ?? string.Empty,
            DanTocNgh = student.DanTocNgh ?? string.Empty,
            TonGiaoNgh = student.TonGiaoNgh ?? string.Empty,
            SdtNgh = student.SdtNgh ?? string.Empty,
            EmailNgh = student.EmailNgh ?? string.Empty,
            DiaChiThuongTruNgh = student.DiaChiThuongTruNgh ?? string.Empty,
            CongViecNgh = student.CongViecNgh ?? string.Empty,
            
            // Thông tin khẩn cấp
            ThongTinNguoiCanBaoTin = student.ThongTinNguoiCanBaoTin ?? string.Empty,
            SoDienThoaiBaoTin = student.SoDienThoaiBaoTin ?? string.Empty,
            
            // Ảnh thẻ
            AnhTheUrl = student.AnhTheUrl ?? string.Empty
        };
    }
}

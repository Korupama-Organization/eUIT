using eUIT.API.DTOs;

namespace eUIT.API.Services;

/// <summary>
/// Interface định nghĩa các phương thức tra cứu sinh viên
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Tìm kiếm sinh viên theo nhiều tiêu chí
    /// Hỗ trợ phân trang
    /// </summary>
    Task<(IEnumerable<StudentSearchDTO> students, int totalCount)> SearchStudentsAsync(StudentSearchRequestDTO searchRequest);
    
    /// <summary>
    /// Lấy thông tin chi tiết sinh viên theo MSSV
    /// </summary>
    Task<StudentDTO?> GetStudentByIdAsync(int studentId);
}

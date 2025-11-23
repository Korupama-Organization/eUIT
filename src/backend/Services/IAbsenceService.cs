using eUIT.API.DTOs;

namespace eUIT.API.Services;

/// <summary>
/// Interface định nghĩa các phương thức xử lý nghiệp vụ cho Absence
/// </summary>
public interface IAbsenceService
{
    /// <summary>
    /// Lấy danh sách tất cả thông tin nghỉ dạy
    /// </summary>
    Task<IEnumerable<AbsenceDTO>> GetAllAbsencesAsync();
    
    /// <summary>
    /// Lấy thông tin nghỉ dạy theo ID
    /// </summary>
    Task<AbsenceDTO?> GetAbsenceByIdAsync(int id);
    
    /// <summary>
    /// Tạo mới thông tin nghỉ dạy
    /// </summary>
    Task<AbsenceDTO> CreateAbsenceAsync(AbsenceCreateDTO createDto);
    
    /// <summary>
    /// Cập nhật thông tin nghỉ dạy
    /// </summary>
    Task<AbsenceDTO?> UpdateAbsenceAsync(int id, AbsenceUpdateDTO updateDto);
    
    /// <summary>
    /// Xóa thông tin nghỉ dạy
    /// </summary>
    Task<bool> DeleteAbsenceAsync(int id);
}

using eUIT.API.DTOs;

namespace eUIT.API.Services;

/// <summary>
/// Interface định nghĩa các phương thức xử lý nghiệp vụ cho Announcement
/// </summary>
public interface IAnnouncementService
{
    /// <summary>
    /// Lấy danh sách tất cả thông báo
    /// </summary>
    Task<IEnumerable<AnnouncementDTO>> GetAllAnnouncementsAsync();
    
    /// <summary>
    /// Lấy thông tin thông báo theo ID
    /// </summary>
    Task<AnnouncementDTO?> GetAnnouncementByIdAsync(int id);
    
    /// <summary>
    /// Tạo mới thông báo
    /// </summary>
    Task<AnnouncementDTO> CreateAnnouncementAsync(AnnouncementCreateDTO createDto);
    
    /// <summary>
    /// Cập nhật thông báo
    /// </summary>
    Task<AnnouncementDTO?> UpdateAnnouncementAsync(int id, AnnouncementUpdateDTO updateDto);
    
    /// <summary>
    /// Xóa thông báo
    /// </summary>
    Task<bool> DeleteAnnouncementAsync(int id);
}

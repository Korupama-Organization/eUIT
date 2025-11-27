using eUIT.API.DTOs;

namespace eUIT.API.Services;

/// <summary>
/// Interface định nghĩa các phương thức quản lý thông báo cho giảng viên
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Lấy danh sách tất cả thông báo của giảng viên
    /// Sắp xếp theo ngày gửi mới nhất
    /// </summary>
    Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync();
    
    /// <summary>
    /// Lấy thông tin chi tiết một thông báo theo ID
    /// </summary>
    Task<NotificationDTO?> GetNotificationByIdAsync(int id);
    
    /// <summary>
    /// Lấy thông báo theo ID người nhận (giảng viên)
    /// </summary>
    Task<IEnumerable<NotificationDTO>> GetNotificationsByUserIdAsync(int userId);
}

using Microsoft.EntityFrameworkCore;
using eUIT.API.Data;
using eUIT.API.DTOs;
using eUIT.API.Models;

namespace eUIT.API.Services;

/// <summary>
/// Service xử lý nghiệp vụ cho Notification (Thông báo giảng viên)
/// </summary>
public class NotificationService : INotificationService
{
    private readonly eUITDbContext _context;

    public NotificationService(eUITDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy tất cả thông báo, sắp xếp theo ngày gửi mới nhất
    /// </summary>
    public async Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync()
    {
        var notifications = await _context.thong_bao_giang_vien
            .OrderByDescending(n => n.NgayGui)
            .ThenByDescending(n => n.Id)
            .ToListAsync();

        return notifications.Select(n => MapToDTO(n));
    }

    /// <summary>
    /// Lấy thông báo theo ID
    /// </summary>
    public async Task<NotificationDTO?> GetNotificationByIdAsync(int id)
    {
        var notification = await _context.thong_bao_giang_vien.FindAsync(id);
        
        if (notification == null)
            return null;

        return MapToDTO(notification);
    }

    /// <summary>
    /// Lấy thông báo theo ID người nhận (giảng viên)
    /// </summary>
    public async Task<IEnumerable<NotificationDTO>> GetNotificationsByUserIdAsync(int userId)
    {
        var notifications = await _context.thong_bao_giang_vien
            .Where(n => n.NguoiGuiId == userId)
            .OrderByDescending(n => n.NgayGui)
            .ThenByDescending(n => n.Id)
            .ToListAsync();

        return notifications.Select(n => MapToDTO(n));
    }

    /// <summary>
    /// Helper method để map từ Model sang DTO
    /// </summary>
    private static NotificationDTO MapToDTO(Notification notification)
    {
        return new NotificationDTO
        {
            Id = notification.Id,
            TieuDe = notification.TieuDe ?? string.Empty,
            NoiDung = notification.NoiDung ?? string.Empty,
            NgayGui = notification.NgayGui,
            NguoiGuiId = notification.NguoiGuiId
        };
    }
}

using Microsoft.EntityFrameworkCore;
using eUIT.API.Data;
using eUIT.API.DTOs;
using eUIT.API.Models;

namespace eUIT.API.Services;

/// <summary>
/// Service xử lý nghiệp vụ cho Announcement
/// </summary>
public class AnnouncementService : IAnnouncementService
{
    private readonly eUITDbContext _context;

    public AnnouncementService(eUITDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy tất cả thông báo, sắp xếp theo ngày tạo mới nhất
    /// </summary>
    public async Task<IEnumerable<AnnouncementDTO>> GetAllAnnouncementsAsync()
    {
        var announcements = await _context.thong_bao
            .OrderByDescending(a => a.NgayTao)
            .ThenByDescending(a => a.Id)
            .ToListAsync();

        return announcements.Select(a => MapToDTO(a));
    }

    /// <summary>
    /// Lấy thông báo theo ID
    /// </summary>
    public async Task<AnnouncementDTO?> GetAnnouncementByIdAsync(int id)
    {
        var announcement = await _context.thong_bao.FindAsync(id);
        
        if (announcement == null)
            return null;

        return MapToDTO(announcement);
    }

    /// <summary>
    /// Tạo mới thông báo
    /// </summary>
    public async Task<AnnouncementDTO> CreateAnnouncementAsync(AnnouncementCreateDTO createDto)
    {
        var currentDate = DateTime.Now.Date;

        var announcement = new Announcement
        {
            TieuDe = createDto.TieuDe,
            NoiDung = createDto.NoiDung,
            NgayTao = createDto.NgayTao ?? currentDate,
            NgayCapNhat = createDto.NgayCapNhat ?? currentDate
        };

        _context.thong_bao.Add(announcement);
        await _context.SaveChangesAsync();

        return MapToDTO(announcement);
    }

    /// <summary>
    /// Cập nhật thông báo
    /// </summary>
    public async Task<AnnouncementDTO?> UpdateAnnouncementAsync(int id, AnnouncementUpdateDTO updateDto)
    {
        var announcement = await _context.thong_bao.FindAsync(id);
        
        if (announcement == null)
            return null;

        // Chỉ cập nhật các trường không null
        if (updateDto.TieuDe != null)
            announcement.TieuDe = updateDto.TieuDe;
            
        if (updateDto.NoiDung != null)
            announcement.NoiDung = updateDto.NoiDung;
            
        if (updateDto.NgayTao.HasValue)
            announcement.NgayTao = updateDto.NgayTao.Value;
            
        // Tự động cập nhật NgayCapNhat
        announcement.NgayCapNhat = updateDto.NgayCapNhat ?? DateTime.Now.Date;

        await _context.SaveChangesAsync();

        return MapToDTO(announcement);
    }

    /// <summary>
    /// Xóa thông báo
    /// </summary>
    public async Task<bool> DeleteAnnouncementAsync(int id)
    {
        var announcement = await _context.thong_bao.FindAsync(id);
        
        if (announcement == null)
            return false;

        _context.thong_bao.Remove(announcement);
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Helper method để map từ Model sang DTO
    /// </summary>
    private static AnnouncementDTO MapToDTO(Announcement announcement)
    {
        return new AnnouncementDTO
        {
            Id = announcement.Id,
            TieuDe = announcement.TieuDe ?? string.Empty,
            NoiDung = announcement.NoiDung ?? string.Empty,
            NgayTao = announcement.NgayTao,
            NgayCapNhat = announcement.NgayCapNhat
        };
    }
}

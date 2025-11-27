using Microsoft.AspNetCore.Mvc;
using eUIT.API.DTOs;
using eUIT.API.Services;

namespace eUIT.API.Controllers;

/// <summary>
/// Controller xử lý API thông báo cho Giảng viên
/// </summary>
[Route("api/GiangVien/notifications")]
[ApiController]
public class LecturerNotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public LecturerNotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// GET: api/GiangVien/notifications
    /// Lấy danh sách tất cả thông báo
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var notifications = await _notificationService.GetAllNotificationsAsync();
            
            return Ok(new 
            { 
                success = true, 
                data = notifications,
                count = notifications.Count()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                success = false, 
                message = ex.Message 
            });
        }
    }

    /// <summary>
    /// GET: api/GiangVien/notifications/{id}
    /// Lấy thông tin chi tiết một thông báo
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            
            if (notification == null)
                return NotFound(new 
                { 
                    success = false, 
                    message = $"Không tìm thấy thông báo với ID {id}" 
                });

            return Ok(new 
            { 
                success = true, 
                data = notification 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                success = false, 
                message = ex.Message 
            });
        }
    }

    /// <summary>
    /// GET: api/GiangVien/notifications/user/{userId}
    /// Lấy danh sách thông báo của một giảng viên cụ thể
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        try
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            
            return Ok(new 
            { 
                success = true, 
                data = notifications,
                count = notifications.Count(),
                userId = userId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                success = false, 
                message = ex.Message 
            });
        }
    }
}

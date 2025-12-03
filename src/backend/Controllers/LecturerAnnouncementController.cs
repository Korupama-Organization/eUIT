using Microsoft.AspNetCore.Mvc;
using eUIT.API.DTOs;
using eUIT.API.Services;

namespace eUIT.API.Controllers;

/// <summary>
/// Controller xử lý API cho Thông báo của Giảng viên
/// </summary>
[Route("api/GiangVien/announcement")]
[ApiController]
public class LecturerAnnouncementController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public LecturerAnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    /// <summary>
    /// GET: api/GiangVien/announcement/list
    /// Lấy danh sách tất cả thông báo
    /// </summary>
    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var announcements = await _announcementService.GetAllAnnouncementsAsync();
            return Ok(new 
            { 
                success = true, 
                data = announcements,
                count = announcements.Count()
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
    /// GET: api/GiangVien/announcement/{id}
    /// Lấy thông tin chi tiết một thông báo
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
            
            if (announcement == null)
                return NotFound(new 
                { 
                    success = false, 
                    message = $"Không tìm thấy thông báo với ID {id}" 
                });

            return Ok(new 
            { 
                success = true, 
                data = announcement 
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
    /// POST: api/GiangVien/announcement/create
    /// Tạo mới thông báo
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AnnouncementCreateDTO createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new 
                { 
                    success = false, 
                    errors = ModelState 
                });

            var announcement = await _announcementService.CreateAnnouncementAsync(createDto);
            
            return CreatedAtAction(
                nameof(GetById), 
                new { id = announcement.Id }, 
                new 
                { 
                    success = true, 
                    data = announcement,
                    message = "Tạo thông báo thành công"
                }
            );
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
    /// PUT: api/GiangVien/announcement/{id}/update
    /// Cập nhật thông tin thông báo
    /// </summary>
    [HttpPut("{id}/update")]
    public async Task<IActionResult> Update(int id, [FromBody] AnnouncementUpdateDTO updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new 
                { 
                    success = false, 
                    errors = ModelState 
                });

            var announcement = await _announcementService.UpdateAnnouncementAsync(id, updateDto);
            
            if (announcement == null)
                return NotFound(new 
                { 
                    success = false, 
                    message = $"Không tìm thấy thông báo với ID {id}" 
                });

            return Ok(new 
            { 
                success = true, 
                data = announcement, 
                message = "Cập nhật thông báo thành công" 
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
    /// DELETE: api/GiangVien/announcement/{id}/delete
    /// Xóa thông báo
    /// </summary>
    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _announcementService.DeleteAnnouncementAsync(id);
            
            if (!result)
                return NotFound(new 
                { 
                    success = false, 
                    message = $"Không tìm thấy thông báo với ID {id}" 
                });

            return Ok(new 
            { 
                success = true, 
                message = "Xóa thông báo thành công" 
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

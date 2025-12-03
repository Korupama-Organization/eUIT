using Microsoft.AspNetCore.Mvc;
using eUIT.API.DTOs;
using eUIT.API.Services;

namespace eUIT.API.Controllers;

[Route("api/GiangVien/absence")]
[ApiController]
public class LecturerAbsenceController : ControllerBase
{
    private readonly IAbsenceService _absenceService;

    public LecturerAbsenceController(IAbsenceService absenceService)
    {
        _absenceService = absenceService;
    }

    /// <summary>
    /// GET: api/GiangVien/absence/list
    /// Lấy danh sách tất cả báo nghỉ dạy
    /// </summary>
    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        try
        {
            var absences = await _absenceService.GetAllAbsencesAsync();
            return Ok(new { success = true, data = absences });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// GET: api/GiangVien/absence/{id}
    /// Lấy thông tin chi tiết một báo nghỉ dạy
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var absence = await _absenceService.GetAbsenceByIdAsync(id);
            
            if (absence == null)
                return NotFound(new { success = false, message = $"Không tìm thấy báo nghỉ dạy với ID {id}" });

            return Ok(new { success = true, data = absence });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// POST: api/GiangVien/absence/create
    /// Tạo mới báo nghỉ dạy
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] AbsenceCreateDTO createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, errors = ModelState });

            var absence = await _absenceService.CreateAbsenceAsync(createDto);
            
            return CreatedAtAction(
                nameof(GetById), 
                new { id = absence.Id }, 
                new { success = true, data = absence }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// PUT: api/GiangVien/absence/{id}/update
    /// Cập nhật thông tin báo nghỉ dạy
    /// </summary>
    [HttpPut("{id}/update")]
    public async Task<IActionResult> Update(int id, [FromBody] AbsenceUpdateDTO updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, errors = ModelState });

            var absence = await _absenceService.UpdateAbsenceAsync(id, updateDto);
            
            if (absence == null)
                return NotFound(new { success = false, message = $"Không tìm thấy báo nghỉ dạy với ID {id}" });

            return Ok(new { success = true, data = absence, message = "Cập nhật thành công" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// DELETE: api/GiangVien/absence/{id}/delete
    /// Xóa báo nghỉ dạy
    /// </summary>
    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _absenceService.DeleteAbsenceAsync(id);
            
            if (!result)
                return NotFound(new { success = false, message = $"Không tìm thấy báo nghỉ dạy với ID {id}" });

            return Ok(new { success = true, message = "Xóa thành công" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }
}

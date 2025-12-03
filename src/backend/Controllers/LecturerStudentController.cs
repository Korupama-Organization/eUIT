using Microsoft.AspNetCore.Mvc;
using eUIT.API.DTOs;
using eUIT.API.Services;

namespace eUIT.API.Controllers;

/// <summary>
/// Controller xử lý API tra cứu sinh viên cho Giảng viên
/// </summary>
[Route("api/GiangVien/students")]
[ApiController]
public class LecturerStudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public LecturerStudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// GET: api/GiangVien/students/search
    /// Tìm kiếm sinh viên theo nhiều tiêu chí với phân trang
    /// </summary>
    /// <remarks>
    /// Ví dụ tìm kiếm:
    /// - Tìm theo keyword: ?keyword=nguyen
    /// - Tìm theo MSSV: ?mssv=21520001
    /// - Tìm theo lớp: ?lopSinhHoat=CNTT2021
    /// - Tìm kết hợp: ?keyword=nguyen&amp;lopSinhHoat=CNTT2021&amp;pageSize=10&amp;pageNumber=1
    /// </remarks>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] StudentSearchRequestDTO searchRequest)
    {
        try
        {
            var (students, totalCount) = await _studentService.SearchStudentsAsync(searchRequest);
            
            var totalPages = (int)Math.Ceiling(totalCount / (double)searchRequest.PageSize);
            
            return Ok(new
            {
                success = true,
                data = students,
                pagination = new
                {
                    totalCount,
                    totalPages,
                    currentPage = searchRequest.PageNumber,
                    pageSize = searchRequest.PageSize,
                    hasNextPage = searchRequest.PageNumber < totalPages,
                    hasPreviousPage = searchRequest.PageNumber > 1
                }
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
    /// GET: api/GiangVien/students/{studentId}
    /// Lấy thông tin chi tiết sinh viên theo MSSV
    /// </summary>
    [HttpGet("{studentId}")]
    public async Task<IActionResult> GetById(int studentId)
    {
        try
        {
            var student = await _studentService.GetStudentByIdAsync(studentId);
            
            if (student == null)
                return NotFound(new
                {
                    success = false,
                    message = $"Không tìm thấy sinh viên với MSSV {studentId}"
                });

            return Ok(new
            {
                success = true,
                data = student
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

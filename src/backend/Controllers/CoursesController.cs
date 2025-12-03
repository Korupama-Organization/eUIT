using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using eUIT.API.Data;
using eUIT.API.DTOs;

namespace eUIT.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly eUITDbContext _context;

    public CoursesController(eUITDbContext context)
    {
        _context = context;
    }

    // GET /api/Courses
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetAllCourses()
    {
        var courses = await _context.Courses
            .AsNoTracking()
            .OrderBy(c => c.TenMonHocVn)
            .ToListAsync();

        if (!courses.Any()) return NoContent();

        var dtos = courses.Select(c => new CourseDto
        {
            MaMonHoc = c.MaMonHoc,
            TenMonHocVn = c.TenMonHocVn,
            TenMonHocEn = c.TenMonHocEn,
            ConMoLop = c.ConMoLop,
            KhoaBoMonQuanLy = c.KhoaBoMonQuanLy,
            LoaiMonHoc = c.LoaiMonHoc,
            SoTcLyThuyet = c.SoTcLyThuyet,
            SoTcThucHanh = c.SoTcThucHanh
        }).ToList();

        return Ok(dtos);
    }

    // GET /api/Courses/{maMon}
    [HttpGet("{maMon}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(string maMon)
    {
        var course = await _context.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.MaMonHoc == maMon);

        if (course == null) return NotFound();

        var dto = new CourseDto
        {
            MaMonHoc = course.MaMonHoc,
            TenMonHocVn = course.TenMonHocVn,
            TenMonHocEn = course.TenMonHocEn,
            ConMoLop = course.ConMoLop,
            KhoaBoMonQuanLy = course.KhoaBoMonQuanLy,
            LoaiMonHoc = course.LoaiMonHoc,
            SoTcLyThuyet = course.SoTcLyThuyet,
            SoTcThucHanh = course.SoTcThucHanh
        };

        return Ok(dto);
    }

}

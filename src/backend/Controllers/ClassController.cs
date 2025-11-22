using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using eUIT.API.Data;
using eUIT.API.DTOs;
using Npgsql; 

namespace eUIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ILogger<ClassController> _logger;

    public ClassController(eUITDbContext context, ILogger<ClassController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách lớp học mà giảng viên phụ trách
    /// </summary>
    [HttpGet("my-classes")]
    public async Task<IActionResult> GetMyClasses()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (role == "lecturer")
            {
                // Lấy từ bảng Class/Teaching assignment
                await using var connection = _context.Database.GetDbConnection();
                await connection.OpenAsync();
                await using var cmd = connection.CreateCommand();
                
                cmd.CommandText = @"
                    SELECT c.class_id, c.class_name, c.course_name, c.giang_vien_id,
                           c.number_of_students, c.schedule, c.room, c.semester, c.academic_year
                    FROM classes c
                    WHERE c.giang_vien_id = @giangVienId
                    ORDER BY c.academic_year DESC, c.semester DESC
                ";
                
                var param = cmd.CreateParameter();
                param.ParameterName = "giangVienId";
                param.Value = userId;
                cmd.Parameters.Add(param);

                var classes = new List<ClassDetailDto>();
                await using var reader = await cmd.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    classes.Add(new ClassDetailDto
                    {
                        ClassId = reader["class_id"].ToString(),
                        ClassName = reader["class_name"].ToString(),
                        CourseName = reader["course_name"].ToString(),
                        GiangVienId = reader["giang_vien_id"].ToString(),
                        NumberOfStudents = (int)reader["number_of_students"],
                        Schedule = reader["schedule"].ToString(),
                        Room = reader["room"].ToString(),
                        Semester = reader["semester"].ToString(),
                        AcademicYear = reader["academic_year"].ToString()
                    });
                }

                return Ok(classes);
            }

            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting my classes");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Lấy chi tiết một lớp học
    /// </summary>
    [HttpGet("{classId}")]
    public async Task<IActionResult> GetClassDetail(string classId)
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();
            
            cmd.CommandText = @"
                SELECT c.class_id, c.class_name, c.course_name, c.giang_vien_id,
                       gv.ho_ten as giang_vien_name,
                       c.number_of_students, c.schedule, c.room, c.semester, c.academic_year
                FROM classes c
                LEFT JOIN giang_vien gv ON c.giang_vien_id = gv.ma_giang_vien
                WHERE c.class_id = @classId
            ";
            
            var param = cmd.CreateParameter();
            param.ParameterName = "classId";
            param.Value = classId;
            cmd.Parameters.Add(param);

            await using var reader = await cmd.ExecuteReaderAsync();
            
            if (await reader.ReadAsync())
            {
                var classDetail = new ClassDetailDto
                {
                    ClassId = reader["class_id"].ToString(),
                    ClassName = reader["class_name"].ToString(),
                    CourseName = reader["course_name"].ToString(),
                    GiangVienId = reader["giang_vien_id"].ToString(),
                    GiangVienName = reader["giang_vien_name"]?.ToString() ?? "",
                    NumberOfStudents = (int)reader["number_of_students"],
                    Schedule = reader["schedule"].ToString(),
                    Room = reader["room"].ToString(),
                    Semester = reader["semester"].ToString(),
                    AcademicYear = reader["academic_year"].ToString()
                };

                return Ok(classDetail);
            }

            return NotFound(new { message = "Class not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting class detail");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Lấy danh sách phòng học
    /// </summary>
    [HttpGet("rooms/list")]
    public async Task<IActionResult> GetRooms()
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();
            
            cmd.CommandText = @"
                SELECT DISTINCT room
                FROM classes
                WHERE room IS NOT NULL
                ORDER BY room
            ";

            var rooms = new List<string>();
            await using var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var room = reader["room"]?.ToString();
                if (!string.IsNullOrEmpty(room))
                {
                    rooms.Add(room);
                }
            }


            return Ok(rooms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting rooms");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

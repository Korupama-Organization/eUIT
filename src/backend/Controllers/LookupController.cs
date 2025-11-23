using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Npgsql;
using eUIT.API.Data;
using eUIT.API.DTOs;

namespace eUIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LookupController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ILogger<LookupController> _logger;

    public LookupController(eUITDbContext context, ILogger<LookupController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("categories")]
    public IActionResult GetCategories()
    {
        try
        {
            var categories = new
            {
                classes = "Classes",
                homeroomClasses = "Homeroom Classes",
                students = "Students"
            };

            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("classes")]
    public async Task<IActionResult> SearchClasses([FromQuery] string keyword = "")
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            var searchKeyword = $"%{keyword}%";

            cmd.CommandText = @"
                SELECT class_id, class_name, course_name, giang_vien_id, 
                       number_of_students, semester, academic_year
                FROM classes
                WHERE class_id ILIKE @keyword OR class_name ILIKE @keyword OR course_name ILIKE @keyword
                ORDER BY class_id
                LIMIT 50
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@keyword", searchKeyword));

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
                    Semester = reader["semester"].ToString(),
                    AcademicYear = reader["academic_year"].ToString()
                });
            }

            return Ok(classes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching classes");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("classes/{classId}")]
    public async Task<IActionResult> GetClassDetail(string classId)
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT c.class_id, c.class_name, c.course_name, c.giang_vien_id,
                       gv.ho_ten as giang_vien_name, c.number_of_students,
                       c.schedule, c.room, c.semester, c.academic_year
                FROM classes c
                LEFT JOIN giang_vien gv ON c.giang_vien_id = gv.ma_giang_vien
                WHERE c.class_id = @classId
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@classId", classId ?? (object)DBNull.Value));

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

    [HttpGet("students")]
    public async Task<IActionResult> SearchStudents([FromQuery] string keyword = "", [FromQuery] string classId = "")
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            var searchKeyword = $"%{keyword}%";

            cmd.CommandText = @"
                SELECT DISTINCT sv.mssv, sv.ho_ten, sv.email_ca_nhan, sv.so_dien_thoai, sv.lop_sinh_hoat
                FROM sinh_vien sv
                WHERE (
                CAST(sv.mssv AS TEXT) ILIKE @keyword OR 
                sv.ho_ten ILIKE @keyword OR 
                sv.email_ca_nhan ILIKE @keyword
                )
                AND (@classId = '' OR sv.lop_sinh_hoat = @classId)
                ORDER BY sv.ho_ten
                LIMIT 100
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@keyword", searchKeyword));
            cmd.Parameters.Add(new NpgsqlParameter("@classId", classId ?? ""));

            var students = new List<StudentSearchDto>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                students.Add(new StudentSearchDto
                {
                    StudentId = reader["student_id"].ToString(),
                    StudentName = reader["student_name"].ToString(),
                    Email = reader["email"].ToString(),
                    Phone = reader["phone"]?.ToString(),
                    ClassId = reader["class_id"]?.ToString()
                });
            }

            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching students");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("students/{studentId}")]
    public async Task<IActionResult> GetStudentDetail(string studentId)
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
               SELECT sv.mssv, sv.ho_ten, sv.email_ca_nhan, sv.so_dien_thoai,
             sv.ngay_sinh, sv.dia_chi_thuong_tru
             FROM sinh_vien sv
             WHERE CAST(sv.mssv AS TEXT) = @studentId
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@studentId", studentId ?? (object)DBNull.Value));

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var student = new
                {
                    StudentId = reader["mssv"].ToString(),
                    StudentName = reader["ho_ten"].ToString(),
                    Email = reader["email_ca_nhan"].ToString(),
                    Phone = reader["so_dien_thoai"]?.ToString(),
                    DateOfBirth = reader["ngay_sinh"],
                    Address = reader["dia_chi_thuong_tru"]?.ToString()
                };

                return Ok(student);
            }

            return NotFound(new { message = "Student not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student detail");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

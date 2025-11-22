using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Npgsql;
using eUIT.API.Data;

namespace eUIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class ScheduleController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ILogger<ScheduleController> _logger;

    public ScheduleController(eUITDbContext context, ILogger<ScheduleController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("categories")]
    public IActionResult GetScheduleCategories()
    {
        try
        {
            var categories = new
            {
                teaching = "Teaching Schedule",
                exam = "Exam Schedule",
                meetings = "Meetings"
            };

            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting schedule categories");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("teaching")]
    public async Task<IActionResult> GetTeachingSchedule([FromQuery] string giangVienId, [FromQuery] int? month, [FromQuery] int? year)
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            var currentDate = DateTime.Now;
            var queryMonth = month ?? currentDate.Month;
            var queryYear = year ?? currentDate.Year;

            cmd.CommandText = @"
                SELECT c.class_id, c.class_name, c.course_name, c.schedule, c.room,
                       c.semester, c.academic_year, c.giang_vien_id
                FROM classes c
                WHERE c.giang_vien_id = @giangVienId
                AND EXTRACT(YEAR FROM NOW()) = @year
                ORDER BY c.class_id
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@giangVienId", giangVienId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@year", queryYear));

            var schedule = new List<object>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                schedule.Add(new
                {
                    ClassId = reader["class_id"].ToString(),
                    ClassName = reader["class_name"].ToString(),
                    CourseName = reader["course_name"].ToString(),
                    Schedule = reader["schedule"].ToString(),
                    Room = reader["room"].ToString(),
                    Semester = reader["semester"].ToString(),
                    AcademicYear = reader["academic_year"].ToString()
                });
            }

            return Ok(schedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting teaching schedule");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("exams")]
    public async Task<IActionResult> GetExamSchedule([FromQuery] string semester, [FromQuery] string academicYear)
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT id, class_id, exam_date, room, invigilators, duration
                FROM exams
                WHERE semester = @semester AND academic_year = @academicYear
                ORDER BY exam_date
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@semester", semester ?? ""));
            cmd.Parameters.Add(new NpgsqlParameter("@academicYear", academicYear ?? ""));

            var exams = new List<object>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                exams.Add(new
                {
                    Id = reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    ExamDate = reader["exam_date"],
                    Room = reader["room"].ToString(),
                    Invigilators = reader["invigilators"].ToString(),
                    Duration = reader["duration"]
                });
            }

            return Ok(exams);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting exam schedule");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("semesters")]
    public async Task<IActionResult> GetSemesters()
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT DISTINCT semester, academic_year
                FROM classes
                ORDER BY academic_year DESC, semester DESC
            ";

            var semesters = new List<object>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                semesters.Add(new
                {
                    Semester = reader["semester"].ToString(),
                    AcademicYear = reader["academic_year"].ToString()
                });
            }

            return Ok(semesters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting semesters");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("meetings")]
    public async Task<IActionResult> GetMeetings()
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT id, title, description, meeting_date, location, duration
                FROM meetings
                ORDER BY meeting_date DESC
                LIMIT 100
            ";

            var meetings = new List<object>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                meetings.Add(new
                {
                    Id = reader["id"],
                    Title = reader["title"].ToString(),
                    Description = reader["description"].ToString(),
                    MeetingDate = reader["meeting_date"],
                    Location = reader["location"].ToString(),
                    Duration = reader["duration"]
                });
            }

            return Ok(meetings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting meetings");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

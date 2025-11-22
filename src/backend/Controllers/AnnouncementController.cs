using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Data.Common;
using Npgsql;
using eUIT.API.Data;
using eUIT.API.DTOs;
using eUIT.API.Models;

namespace eUIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnnouncementController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ILogger<AnnouncementController> _logger;

    public AnnouncementController(eUITDbContext context, ILogger<AnnouncementController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementCreateDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "lecturer")
                return Forbid("Only lecturers can create announcements");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(dto.ClassId) || string.IsNullOrEmpty(dto.Title))
                return BadRequest(new { message = "Missing required fields" });

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            var publishDate = dto.PublishedDate ?? DateTime.UtcNow;

            cmd.CommandText = @"
                INSERT INTO announcements (class_id, title, content, created_by, published_date, created_at)
                VALUES (@classId, @title, @content, @createdBy, @publishedDate, NOW())
                RETURNING id, class_id, title, content, created_by, published_date, created_at
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@classId", dto.ClassId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@title", dto.Title ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@content", dto.Content as object ?? DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@createdBy", userId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@publishedDate", publishDate));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = new AnnouncementResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    Title = reader["title"].ToString(),
                    Content = reader["content"]?.ToString(),
                    CreatedBy = reader["created_by"].ToString(),
                    PublishedDate = (DateTime)reader["published_date"],
                    CreatedAt = (DateTime)reader["created_at"]
                };

                _logger.LogInformation("Announcement created by {UserId} for class {ClassId}", userId, dto.ClassId);
                return CreatedAtAction(nameof(GetAnnouncementDetail), new { id = result.Id }, result);
            }

            return StatusCode(500, new { message = "Failed to create announcement" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating announcement");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("class/{classId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAnnouncementsByClass(string classId)
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT id, class_id, title, content, created_by, published_date, created_at
                FROM announcements
                WHERE class_id = @classId
                ORDER BY published_date DESC
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@classId", classId ?? (object)DBNull.Value));

            var announcements = new List<AnnouncementResponseDto>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                announcements.Add(new AnnouncementResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    Title = reader["title"].ToString(),
                    Content = reader["content"]?.ToString(),
                    CreatedBy = reader["created_by"].ToString(),
                    PublishedDate = (DateTime)reader["published_date"],
                    CreatedAt = (DateTime)reader["created_at"]
                });
            }

            return Ok(announcements);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting announcements");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("my-announcements")]
    public async Task<IActionResult> GetMyAnnouncements()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT id, class_id, title, content, created_by, published_date, created_at
                FROM announcements
                WHERE created_by = @createdBy
                ORDER BY published_date DESC
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@createdBy", userId));

            var announcements = new List<AnnouncementResponseDto>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                announcements.Add(new AnnouncementResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    Title = reader["title"].ToString(),
                    Content = reader["content"]?.ToString(),
                    CreatedBy = reader["created_by"].ToString(),
                    PublishedDate = (DateTime)reader["published_date"],
                    CreatedAt = (DateTime)reader["created_at"]
                });
            }

            return Ok(announcements);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting my announcements");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAnnouncementDetail(int id)
    {
        try
        {
            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT id, class_id, title, content, created_by, published_date, created_at
                FROM announcements
                WHERE id = @id
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var result = new AnnouncementResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    Title = reader["title"].ToString(),
                    Content = reader["content"]?.ToString(),
                    CreatedBy = reader["created_by"].ToString(),
                    PublishedDate = (DateTime)reader["published_date"],
                    CreatedAt = (DateTime)reader["created_at"]
                };

                return Ok(result);
            }

            return NotFound(new { message = "Announcement not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting announcement detail");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteAnnouncement(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT created_by FROM announcements WHERE id = @id";
            cmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return NotFound(new { message = "Announcement not found" });

            var createdBy = reader["created_by"].ToString();
            if (createdBy != userId && User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return Forbid();

            await reader.CloseAsync();
            await cmd.DisposeAsync();

            await using var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM announcements WHERE id = @id";
            deleteCmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await deleteCmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Announcement {Id} deleted by {UserId}", id, userId);
            return Ok(new { message = "Announcement deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting announcement");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

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
public class MakeupController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ILogger<MakeupController> _logger;

    public MakeupController(eUITDbContext context, ILogger<MakeupController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMakeup([FromBody] MakeupCreateDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "lecturer")
                return Forbid("Only lecturers can create makeup requests");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(dto.ClassId))
                return BadRequest(new { message = "Missing required fields" });

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO makeup_classes (giang_vien_id, class_id, makeup_date, room_id, reason, status, created_at)
                VALUES (@giangVienId, @classId, @makeupDate, @roomId, @reason, 'submitted', NOW())
                RETURNING id, giang_vien_id, class_id, makeup_date, room_id, reason, status, created_at
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@giangVienId", userId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@classId", dto.ClassId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@makeupDate", dto.MakeupDate));
            cmd.Parameters.Add(new NpgsqlParameter("@roomId", dto.RoomId as object ?? DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@reason", dto.Reason as object ?? DBNull.Value));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = new MakeupResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    MakeupDate = (DateTime)reader["makeup_date"],
                    RoomId = reader["room_id"]?.ToString(),
                    Reason = reader["reason"]?.ToString(),
                    Status = reader["status"]?.ToString(),
                    CreatedAt = (DateTime)reader["created_at"]
                };

                _logger.LogInformation("Makeup created by {UserId} for class {ClassId}", userId, dto.ClassId);
                return CreatedAtAction(nameof(GetMakeupDetail), new { id = result.Id }, result);
            }

            return StatusCode(500, new { message = "Failed to create makeup" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating makeup");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetMakeupList()
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
                SELECT m.id, m.class_id, c.class_name, m.makeup_date, m.room_id, 
                       m.reason, m.status, m.created_at, m.updated_at
                FROM makeup_classes m
                JOIN classes c ON m.class_id = c.class_id
                WHERE m.giang_vien_id = @giangVienId
                ORDER BY m.created_at DESC
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@giangVienId", userId));

            var makeups = new List<MakeupResponseDto>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                makeups.Add(new MakeupResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    ClassName = reader["class_name"]?.ToString(),
                    MakeupDate = (DateTime)reader["makeup_date"],
                    RoomId = reader["room_id"]?.ToString(),
                    Reason = reader["reason"]?.ToString(),
                    Status = reader["status"]?.ToString(),
                    CreatedAt = (DateTime)reader["created_at"],
                    UpdatedAt = reader["updated_at"] as DateTime?
                });
            }

            return Ok(makeups);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting makeup list");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMakeupDetail(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT m.id, m.class_id, c.class_name, m.makeup_date, m.room_id,
                       m.reason, m.status, m.created_at, m.updated_at, m.giang_vien_id
                FROM makeup_classes m
                JOIN classes c ON m.class_id = c.class_id
                WHERE m.id = @id
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@id", id));
            
            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var makeupOwnerId = reader["giang_vien_id"].ToString();
                if (User.FindFirst(ClaimTypes.Role)?.Value != "admin" && makeupOwnerId != userId)
                    return Forbid();

                var result = new MakeupResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    ClassName = reader["class_name"]?.ToString(),
                    MakeupDate = (DateTime)reader["makeup_date"],
                    RoomId = reader["room_id"]?.ToString(),
                    Reason = reader["reason"]?.ToString(),
                    Status = reader["status"]?.ToString(),
                    CreatedAt = (DateTime)reader["created_at"],
                    UpdatedAt = reader["updated_at"] as DateTime?
                };

                return Ok(result);
            }

            return NotFound(new { message = "Makeup not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting makeup detail");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPut("{id}/update")]
    public async Task<IActionResult> UpdateMakeup(int id, [FromBody] MakeupUpdateDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT giang_vien_id, status FROM makeup_classes WHERE id = @id";
            cmd.Parameters.Add(new NpgsqlParameter("@id", id));
            
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return NotFound(new { message = "Makeup not found" });

            var ownerId = reader["giang_vien_id"].ToString();
            var currentStatus = reader["status"].ToString();

            if (ownerId != userId && User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return Forbid();

            if (currentStatus != "draft" && User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return BadRequest(new { message = "Cannot update non-draft makeup" });

            await reader.CloseAsync();
            await cmd.DisposeAsync();

            await using var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = @"
                UPDATE makeup_classes
                SET makeup_date = COALESCE(@makeupDate, makeup_date),
                    room_id = COALESCE(@roomId, room_id),
                    reason = COALESCE(@reason, reason),
                    status = COALESCE(@status, status),
                    updated_at = NOW()
                WHERE id = @id
                RETURNING id, class_id, makeup_date, room_id, reason, status, created_at, updated_at
            ";

            updateCmd.Parameters.Add(new NpgsqlParameter("@id", id));
            updateCmd.Parameters.Add(new NpgsqlParameter("@makeupDate", dto.MakeupDate as object ?? DBNull.Value));
            updateCmd.Parameters.Add(new NpgsqlParameter("@roomId", dto.RoomId as object ?? DBNull.Value));
            updateCmd.Parameters.Add(new NpgsqlParameter("@reason", dto.Reason as object ?? DBNull.Value));
            updateCmd.Parameters.Add(new NpgsqlParameter("@status", dto.Status as object ?? DBNull.Value));

            await using var updateReader = await updateCmd.ExecuteReaderAsync();
            if (await updateReader.ReadAsync())
            {
                var result = new MakeupResponseDto
                {
                    Id = (int)updateReader["id"],
                    ClassId = updateReader["class_id"].ToString(),
                    MakeupDate = (DateTime)updateReader["makeup_date"],
                    RoomId = updateReader["room_id"]?.ToString(),
                    Reason = updateReader["reason"]?.ToString(),
                    Status = updateReader["status"]?.ToString(),
                    CreatedAt = (DateTime)updateReader["created_at"],
                    UpdatedAt = updateReader["updated_at"] as DateTime?
                };

                _logger.LogInformation("Makeup {Id} updated by {UserId}", id, userId);
                return Ok(result);
            }

            return StatusCode(500, new { message = "Failed to update makeup" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating makeup");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteMakeup(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT giang_vien_id, status FROM makeup_classes WHERE id = @id";
            cmd.Parameters.Add(new NpgsqlParameter("@id", id));
            
            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return NotFound(new { message = "Makeup not found" });

            var ownerId = reader["giang_vien_id"].ToString();
            var currentStatus = reader["status"].ToString();

            if (ownerId != userId && User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return Forbid();

            if (currentStatus != "draft" && User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return BadRequest(new { message = "Cannot delete non-draft makeup" });

            await reader.CloseAsync();
            await cmd.DisposeAsync();

            await using var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM makeup_classes WHERE id = @id";
            deleteCmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await deleteCmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Makeup {Id} deleted by {UserId}", id, userId);
            return Ok(new { message = "Makeup deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting makeup");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

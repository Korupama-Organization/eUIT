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
public class AbsenceController : ControllerBase
{
    private readonly eUITDbContext _context;
    private readonly ILogger<AbsenceController> _logger;

    public AbsenceController(eUITDbContext context, ILogger<AbsenceController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAbsence([FromBody] AbsenceCreateDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "lecturer")
                return Forbid("Only lecturers can create absence requests");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(dto.ClassId))
                return BadRequest(new { message = "Missing required fields" });

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO absences (giang_vien_id, class_id, absence_date, reason, type, status, created_at)
                VALUES (@giangVienId, @classId, @absenceDate, @reason, @type, 'submitted', NOW())
                RETURNING id, giang_vien_id, class_id, absence_date, reason, type, status, created_at
            ";

            cmd.Parameters.Add(new NpgsqlParameter("@giangVienId", userId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@classId", dto.ClassId ?? (object)DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@absenceDate", dto.AbsenceDate));
            cmd.Parameters.Add(new NpgsqlParameter("@reason", dto.Reason as object ?? DBNull.Value));
            cmd.Parameters.Add(new NpgsqlParameter("@type", dto.Type as object ?? "personal"));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var result = new AbsenceResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    AbsenceDate = (DateTime)reader["absence_date"],
                    Reason = reader["reason"]?.ToString(),
                    Type = reader["type"]?.ToString(),
                    Status = reader["status"]?.ToString(),
                    CreatedAt = (DateTime)reader["created_at"]
                };

                _logger.LogInformation("Absence created by {UserId} for class {ClassId}", userId, dto.ClassId);
                return CreatedAtAction(nameof(GetAbsenceDetail), new { id = result.Id }, result);
            }

            return StatusCode(500, new { message = "Failed to create absence" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating absence");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAbsenceList()
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
            SELECT a.id, a.giang_vien_id, a.class_id, NULL::text as class_name, a.absence_date, a.reason,
                a.type, a.status, a.created_at, a.updated_at
            FROM absences a
            WHERE a.giang_vien_id = @giangVienId
            ORDER BY a.created_at DESC
";


            cmd.Parameters.Add(new NpgsqlParameter("@giangVienId", userId));

            var absences = new List<AbsenceResponseDto>();
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                absences.Add(new AbsenceResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    ClassName = reader["class_name"]?.ToString(),
                    AbsenceDate = (DateTime)reader["absence_date"],
                    Reason = reader["reason"]?.ToString(),
                    Type = reader["type"]?.ToString(),
                    Status = reader["status"]?.ToString(),
                    CreatedAt = (DateTime)reader["created_at"],
                    UpdatedAt = reader["updated_at"] as DateTime?
                });
            }

            return Ok(absences);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting absence list");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAbsenceDetail(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = @"
                SELECT a.id, a.giang_vien_id, a.class_id, NULL::text as class_name, a.absence_date, a.reason,
                    a.type, a.status, a.created_at, a.updated_at
                FROM absences a
                WHERE a.id = @id
";


            cmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var absenceOwnerId = reader["giang_vien_id"].ToString();
                if (User.FindFirst(ClaimTypes.Role)?.Value != "admin" && absenceOwnerId != userId)
                    return Forbid();

                var result = new AbsenceResponseDto
                {
                    Id = (int)reader["id"],
                    ClassId = reader["class_id"].ToString(),
                    ClassName = reader["class_name"]?.ToString(),
                    AbsenceDate = (DateTime)reader["absence_date"],
                    Reason = reader["reason"]?.ToString(),
                    Type = reader["type"]?.ToString(),
                    Status = reader["status"]?.ToString(),
                    CreatedAt = (DateTime)reader["created_at"],
                    UpdatedAt = reader["updated_at"] as DateTime?
                };

                return Ok(result);
            }

            return NotFound(new { message = "Absence not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting absence detail");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPut("{id}/update")]
    public async Task<IActionResult> UpdateAbsence(int id, [FromBody] AbsenceUpdateDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT giang_vien_id FROM absences WHERE id = @id";
            cmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return NotFound(new { message = "Absence not found" });

            var ownerId = reader["giang_vien_id"].ToString();
            if (ownerId != userId && User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return Forbid();

            await reader.CloseAsync();
            await cmd.DisposeAsync();

            await using var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = @"
                UPDATE absences
                SET absence_date = COALESCE(@absenceDate, absence_date),
                    reason = COALESCE(@reason, reason),
                    type = COALESCE(@type, type),
                    status = COALESCE(@status, status),
                    updated_at = NOW()
                WHERE id = @id
                RETURNING id, class_id, absence_date, reason, type, status, created_at, updated_at
            ";

            updateCmd.Parameters.Add(new NpgsqlParameter("@id", id));
            updateCmd.Parameters.Add(new NpgsqlParameter("@absenceDate", dto.AbsenceDate as object ?? DBNull.Value));
            updateCmd.Parameters.Add(new NpgsqlParameter("@reason", dto.Reason as object ?? DBNull.Value));
            updateCmd.Parameters.Add(new NpgsqlParameter("@type", dto.Type as object ?? DBNull.Value));
            updateCmd.Parameters.Add(new NpgsqlParameter("@status", dto.Status as object ?? DBNull.Value));

            await using var updateReader = await updateCmd.ExecuteReaderAsync();
            if (await updateReader.ReadAsync())
            {
                var result = new AbsenceResponseDto
                {
                    Id = (int)updateReader["id"],
                    ClassId = updateReader["class_id"].ToString(),
                    AbsenceDate = (DateTime)updateReader["absence_date"],
                    Reason = updateReader["reason"]?.ToString(),
                    Type = updateReader["type"]?.ToString(),
                    Status = updateReader["status"]?.ToString(),
                    CreatedAt = (DateTime)updateReader["created_at"],
                    UpdatedAt = updateReader["updated_at"] as DateTime?
                };

                _logger.LogInformation("Absence {Id} updated by {UserId}", id, userId);
                return Ok(result);
            }

            return StatusCode(500, new { message = "Failed to update absence" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating absence");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpDelete("{id}/delete")]
    public async Task<IActionResult> DeleteAbsence(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await using var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            await using var cmd = connection.CreateCommand();

            cmd.CommandText = "SELECT giang_vien_id FROM absences WHERE id = @id";
            cmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return NotFound(new { message = "Absence not found" });

            var ownerId = reader["giang_vien_id"].ToString();
            if (ownerId != userId && User.FindFirst(ClaimTypes.Role)?.Value != "admin")
                return Forbid();

            await reader.CloseAsync();
            await cmd.DisposeAsync();

            await using var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM absences WHERE id = @id";
            deleteCmd.Parameters.Add(new NpgsqlParameter("@id", id));

            await deleteCmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Absence {Id} deleted by {UserId}", id, userId);
            return Ok(new { message = "Absence deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting absence");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

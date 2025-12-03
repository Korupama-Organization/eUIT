using Microsoft.EntityFrameworkCore;
using eUIT.API.Data;
using eUIT.API.DTOs;
using eUIT.API.Models;

namespace eUIT.API.Services;

/// <summary>
/// Service xử lý nghiệp vụ cho Absence
/// </summary>
public class AbsenceService : IAbsenceService
{
    private readonly eUITDbContext _context;

    public AbsenceService(eUITDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AbsenceDTO>> GetAllAbsencesAsync()
    {
        var absences = await _context.bao_nghi_day
            .OrderByDescending(a => a.NgayNghi)
            .ToListAsync();

        return absences.Select(a => MapToDTO(a));
    }

    public async Task<AbsenceDTO?> GetAbsenceByIdAsync(int id)
    {
        var absence = await _context.bao_nghi_day.FindAsync(id);
        
        if (absence == null)
            return null;

        return MapToDTO(absence);
    }

    public async Task<AbsenceDTO> CreateAbsenceAsync(AbsenceCreateDTO createDto)
    {
        var absence = new Absence
        {
            MaLop = createDto.MaLop,
            MaGiangVien = createDto.MaGiangVien,
            LyDo = createDto.LyDo,
            NgayNghi = createDto.NgayNghi,
            TinhTrang = createDto.TinhTrang
        };

        _context.bao_nghi_day.Add(absence);
        await _context.SaveChangesAsync();

        return MapToDTO(absence);
    }

    public async Task<AbsenceDTO?> UpdateAbsenceAsync(int id, AbsenceUpdateDTO updateDto)
    {
        var absence = await _context.bao_nghi_day.FindAsync(id);
        
        if (absence == null)
            return null;

        // Chỉ cập nhật các trường không null
        if (updateDto.MaLop != null)
            absence.MaLop = updateDto.MaLop;
            
        if (updateDto.MaGiangVien != null)
            absence.MaGiangVien = updateDto.MaGiangVien;
            
        if (updateDto.LyDo != null)
            absence.LyDo = updateDto.LyDo;
            
        if (updateDto.NgayNghi.HasValue)
            absence.NgayNghi = updateDto.NgayNghi.Value;
            
        if (updateDto.TinhTrang != null)
            absence.TinhTrang = updateDto.TinhTrang;

        await _context.SaveChangesAsync();

        return MapToDTO(absence);
    }

    public async Task<bool> DeleteAbsenceAsync(int id)
    {
        var absence = await _context.bao_nghi_day.FindAsync(id);
        
        if (absence == null)
            return false;

        _context.bao_nghi_day.Remove(absence);
        await _context.SaveChangesAsync();

        return true;
    }

    // Helper method để map từ Model sang DTO
    private static AbsenceDTO MapToDTO(Absence absence)
    {
        return new AbsenceDTO
        {
            Id = absence.Id,
            MaLop = absence.MaLop,
            MaGiangVien = absence.MaGiangVien,
            LyDo = absence.LyDo,
            NgayNghi = absence.NgayNghi,
            TinhTrang = absence.TinhTrang
        };
    }
}

namespace eUIT.API.DTOs;

public class AbsenceResponseDto
{
    public int Id { get; set; }
    public string? ClassId { get; set; }
    public string? ClassName { get; set; }
    public DateTime AbsenceDate { get; set; }
    public string? Reason { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

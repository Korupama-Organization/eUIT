namespace eUIT.API.DTOs;

public class AbsenceUpdateDto
{
    public DateTime? AbsenceDate { get; set; }
    public string? Reason { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
}

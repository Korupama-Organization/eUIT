namespace eUIT.API.DTOs;

public class AbsenceCreateDto
{
    public string? ClassId { get; set; }
    public DateTime AbsenceDate { get; set; }
    public string? Reason { get; set; }
    public string? Type { get; set; }
}

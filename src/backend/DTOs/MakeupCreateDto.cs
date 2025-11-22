namespace eUIT.API.DTOs;

public class MakeupCreateDto
{
    public string? ClassId { get; set; }
    public DateTime MakeupDate { get; set; }
    public string? RoomId { get; set; }
    public string? Reason { get; set; }
}

namespace eUIT.API.DTOs;

public class MakeupResponseDto
{
    public int Id { get; set; }
    public string? ClassId { get; set; }
    public string? ClassName { get; set; }
    public DateTime MakeupDate { get; set; }
    public string? RoomId { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

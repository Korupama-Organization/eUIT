namespace eUIT.API.DTOs;

public class AnnouncementResponseDto
{
    public int Id { get; set; }
    public string? ClassId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime PublishedDate { get; set; }
    public DateTime CreatedAt { get; set; }
}

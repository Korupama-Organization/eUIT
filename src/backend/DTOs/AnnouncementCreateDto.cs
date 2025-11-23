namespace eUIT.API.DTOs;

public class AnnouncementCreateDto
{
    public string? ClassId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? PublishedDate { get; set; }
}

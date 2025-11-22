namespace eUIT.API.Models;

public class Meeting
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime MeetingDate { get; set; }
    public string? Location { get; set; }
    public int? Duration { get; set; }
}

namespace eUIT.API.Models;

public class Exam
{
    public int Id { get; set; }
    public string? ClassId { get; set; }
    public DateTime ExamDate { get; set; }
    public string? Room { get; set; }
    public string? Invigilators { get; set; }
    public int? Duration { get; set; }
    public string? Semester { get; set; }
    public string? AcademicYear { get; set; }
}

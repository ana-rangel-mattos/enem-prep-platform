namespace EnemPrep.Domain.DTOS;

public class GetExamQuestionDto
{
    public Guid ExamQuestionId { get; set; }
    public int Year { get; set; }
    public string Context { get; set; } = string.Empty;
    public string Enunciation { get; set; } = string.Empty;
    public Dictionary<char, string> Alternatives { get; set; } = new Dictionary<char, string>();
}
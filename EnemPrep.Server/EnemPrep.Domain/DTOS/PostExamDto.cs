using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.DTOS;

public class PostExamDto
{
    private const double TimeSpentForQuestionMinutes = 3.5;
    
    public int TotalQuestions { get; set; }
    public List<SubjectName> Areas { get; set; } = new List<SubjectName>();
    public Language? ExamLanguage { get; set; }

    public TimeSpan GetMaxSpentTime()
    {
        double totalMinutes = TimeSpentForQuestionMinutes * TotalQuestions;
        
        return TimeSpan.FromMinutes(totalMinutes);
    }
}
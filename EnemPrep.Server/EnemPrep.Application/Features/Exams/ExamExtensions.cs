using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Exams;

public static class ExamExtensions
{
    public static GetExamDto ConvertToDto(this Exam exam)
    {
        return new GetExamDto
        {
            ExamId = exam.ExamId,
            UserId = exam.UserId,
            ExamYear = exam.ExamYear,
            Title = exam.Title,
            Status = exam.Status.ToString(),
            StatusDescription = exam.Status switch
            {
                ExamStatus.NotStarted => "Not Started",
                ExamStatus.InProgress => "Em Progresso",
                ExamStatus.Finished => "Finalizado",
                ExamStatus.Canceled => "Cancelado",
                _ => "Desconhecido"
            },
            Language = exam.LanguageChoice.ToString(),
            LanguageDescription = exam.LanguageChoice switch
            {
                Language.English => "Inglês",
                Language.Spanish => "Espanhol",
                _ => "Não Informado"
            },
            QuestionsCount = exam.QuestionsCount,
            CorrectQuestionsCount = exam.CorrectQuestionsCount,
            IncorrectQuestionsCount = exam.IncorrectQuestionsCount,
            UnsolvedQuestionsCount = exam.UnsolvedQuestionsCount,
            TotalSpentTime = exam.TotalSpentTime,
            MaxSpentTime = exam.MaxSpentTime,
            EstimatedScore = exam.EstimatedScore,
            CreatedAt = exam.CreatedAt
        };
    }
}
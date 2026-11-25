using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Subjects;

public static class SubjectExtensions
{
    public static GetSubjectDto ToGetSubjectDto(this Subject subject)
    {
        return new GetSubjectDto
        {
            SubjectId = subject.SubjectId,
            Name = ConvertSubjectToString(subject.Name)
        };
    }
    
    private static string ConvertSubjectToString(SubjectName subject)
    {
        return subject switch
        {
            SubjectName.Languages => "Linguagens",
            SubjectName.Humanities => "Ciências Humanas",
            SubjectName.Mathematics => "Matemática",
            SubjectName.NaturalSciences => "Ciências da Natureza",
            _ => throw new ArgumentOutOfRangeException(
                nameof(subject), 
                subject.ToString(), 
                $"{subject.ToString()} is not a valid subject name."
            )
        };
    }
}
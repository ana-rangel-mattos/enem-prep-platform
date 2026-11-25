using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Application.Features.Subjects;

public class GetSubjectDto
{
    public Guid SubjectId { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class PostSubjectDto
{
    [DisplayName("Subject")]
    [Required(ErrorMessage = "{0} is required")]
    public SubjectName SubjectName { get; set; }
}
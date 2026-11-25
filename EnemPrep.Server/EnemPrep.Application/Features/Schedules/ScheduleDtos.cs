using System.ComponentModel.DataAnnotations;
using EnemPrep.Domain.Enums;

namespace EnemPrep.Application.Features.Schedules;

public class PostScheduleDto
{
    [Required(ErrorMessage = "{0} é obrigatório.")]
    [Length(3, 50, ErrorMessage = "{0} deve conter no entre 3 e 50 caracteres.")]
    [Display(Name = "Nome do Cronograma")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Incluir Fim de Semana")]
    public bool HasWeekEnd { get; set; } = false;
}

public class PostScheduleSubjectDto
{
    public Guid SubjectId { get; set; }
    public DayOfTheWeek WeekDay { get; set; }
}

public class UpdateScheduleDto
{
    public string Name { get; set; } = string.Empty;
    public bool HasWeekEnd { get; set; }
}

public class UpdateScheduleSubjectOrderDto
{
    public Guid ScheduleSubjectId { get; set; }
    public DayOfTheWeek NewDay { get; set; }
    public int NewIndex { get; set; }
}

public class GetScheduleDto
{
    public Guid ScheduleId { get; set; }

    public string Name { get; set; } = string.Empty;
}

public class GetScheduleSubjectDto
{
    public Guid ScheduleSubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}

public class GetScheduleDetailsDto
{
    public Guid ScheduleId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool HasWeekEnd { get; set; }
    public Dictionary<DayOfTheWeek, IReadOnlyCollection<GetScheduleSubjectDto>> Schedule { get; set; } = new();
}
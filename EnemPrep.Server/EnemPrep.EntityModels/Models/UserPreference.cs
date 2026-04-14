using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EnemPrep.EntityModels.Enums;

namespace EnemPrep.EntityModels.Models;

public partial class UserPreference
{
    public Guid UserPreferencesId { get; set; }

    public Guid UserId { get; set; }

    public int QuestionsPerDay { get; set; }
    
    [Column("exam_language")]

    public Language ExamLanguage { get; set; } = Language.Ingles;

    [Column("color_scheme")]
    public ColorScheme ColorScheme { get; set; } = ColorScheme.OS;

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}

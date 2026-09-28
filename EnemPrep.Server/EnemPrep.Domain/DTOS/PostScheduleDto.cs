using System.ComponentModel.DataAnnotations;
using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.DTOS;

public class PostScheduleDto
{
    [Required(ErrorMessage = "{0} é obrigatório.")]
    [Length(3, 50, ErrorMessage = "{0} deve conter no entre 3 e 50 caracteres.")]
    [Display(Name = "Nome do Cronograma")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Incluir Fim de Semana")]
    public bool HasWeekEnd { get; set; } = false;
}
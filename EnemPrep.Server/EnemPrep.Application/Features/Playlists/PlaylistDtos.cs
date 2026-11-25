using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EnemPrep.Application.Features.Articles;

namespace EnemPrep.Application.Features.Playlists;

public class GetPlaylistDto
{
    public Guid PlaylistId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}

public class GetPlaylistQuestionDto
{
    public Guid QuestionId { get; set; }
    public int Year { get; set; }
    public string? Enunciation { get; set; } = string.Empty;
    public string? Context { get; set; } = string.Empty;
}

public class GetPlaylistDetailsDto
{
    public Guid PlaylistId { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public ICollection<GetPlaylistQuestionDto> Questions { get; set; } = new List<GetPlaylistQuestionDto>();
    public ICollection<GetArticleDto> Articles { get; set; } = new List<GetArticleDto>();
}

public class PostPlaylistDto
{
    [Required(ErrorMessage = "{0} é mandatório.")]
    [DisplayName("Título")]
    [Length(5, 50, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "{0} é mandatória.")]
    [Length(10, 500, ErrorMessage = "{0} deve ter entre {2} e {1} caracteres.")]
    public string Description { get; set; } = string.Empty;
    
    public bool IsPublic { get; set; }
}
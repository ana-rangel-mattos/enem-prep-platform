using EnemPrep.Application.Features.Questions;
using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Playlists;

public static class PlaylistQuestionExtensions
{
    public static GetPlaylistQuestionDto ToGetPlaylistQuestionDto(this PlaylistQuestion playlistQuestion)
    {
        Question question = playlistQuestion.Question;
        
        return new GetPlaylistQuestionDto
        {
            QuestionId = playlistQuestion.QuestionId,
            Year = question.ExtractYear(),
            Context = question.ExtractContext(),
            Enunciation = question.ExtractEnunciation()
        };
    }
}
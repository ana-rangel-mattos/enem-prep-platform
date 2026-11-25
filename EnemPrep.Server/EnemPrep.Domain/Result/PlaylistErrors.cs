namespace EnemPrep.Domain.Result;

public static class PlaylistErrors
{
    public static Error PlaylistWasNotFound(Guid playlistId) => new Error(
        ErrorNames.PlaylistPlaylistWasNotFound,
        $"Playlist ID {playlistId} was not found.");

    public static readonly Error PlaylistAccessForbidden = new Error(
        ErrorNames.PlaylistPlaylistAccessForbidden,
        "You are not allowed to access this playlist.");
    
    public static readonly Error PlaylistDeleteAccessForbidden = new Error(
        ErrorNames.PlaylistDeleteAccessForbidden,
        "You are not allowed to delete this playlist.");
    
    public static readonly Error PlaylistDeleteQuestionAccessForbidden = new Error(
        ErrorNames.PlaylistDeleteQuestionAccessForbidden,
        "You are not allowed to delete this playlist.");
    
    public static readonly Error PlaylistAddQuestionAccessForbidden = new Error(
        ErrorNames.PlaylistAddQuestionAccessForbidden,
        "You are not allowed to delete this playlist.");

    public static Error PlaylistQuestionNotFound(Guid questionId) => new Error(
        ErrorNames.PlaylistQuestionWasNotFound,
        $"Question ID {questionId} was not found.");
}
using EnemPrep.Domain.Models;

namespace EnemPrep.Application.Features.Playlists;

public static class PlaylistExtensions
{
    public static GetPlaylistDto ToGetPlaylistDto(this Playlist playlist)
    {
        return new GetPlaylistDto
        {
            UserId = playlist.UserId,
            PlaylistId = playlist.PlaylistId,
            Title = playlist.Title,
            Description = playlist.Description,
            IsPublic = playlist.IsPublic
        };
    }
}
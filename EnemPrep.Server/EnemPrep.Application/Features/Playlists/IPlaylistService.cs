using EnemPrep.Application.Common.Models;
using EnemPrep.Application.Common.Querying;
using EnemPrep.Domain.Result;

namespace EnemPrep.Application.Features.Playlists;

public interface IPlaylistService
{
    Task<PagedResponse<GetPlaylistDto>> FetchLoggedUserPlaylistsAsync(QueryFilter filter, CancellationToken cancellationToken = default);
    Task<Result<GetPlaylistDetailsDto>> FetchPlaylistByIdAsync(Guid playlistId, CancellationToken cancellationToken = default);
    Task<Result> CreatePlaylistAsync(PostPlaylistDto request, CancellationToken cancellationToken = default);
    Task<Result> DeletePlaylistAsync(Guid playlistId, CancellationToken cancellationToken = default);
    Task<Result> AddQuestionToPlaylistAsync(Guid playlistId, Guid questionId, CancellationToken cancellationToken = default);
    Task<Result> RemoveQuestionFromPlaylistAsync(Guid playlistId, Guid questionId, CancellationToken cancellationToken);
}
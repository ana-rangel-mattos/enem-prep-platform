using EnemPrep.Application.Common.Querying;
using EnemPrep.Application.Features.Playlists;
using EnemPrep.Domain.Enums;
using EnemPrep.Domain.Result;
using EnemPrep.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnemPrep.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistsController : ControllerBase
{
    private readonly IPlaylistService _playlistService;

    public PlaylistsController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    [HasPermission(Permission.ReadPlaylist)]
    [HttpGet]
    public async Task<IActionResult> GetUserPlaylists(QueryFilter filter, CancellationToken cancellationToken)
    {
        var result = await _playlistService.FetchLoggedUserPlaylistsAsync(filter, cancellationToken);

        return Ok(result);
    }

    [HasPermission(Permission.ReadPlaylist)]
    [HttpGet("{playlistId:guid}")]
    public async Task<IActionResult> GetPlaylistById(Guid playlistId, CancellationToken cancellationToken)
    {
        var result = await _playlistService.FetchPlaylistByIdAsync(playlistId, cancellationToken);

        return result.Match<IActionResult>(
            onSuccess: () => Ok(result.Value),
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.PlaylistPlaylistWasNotFound => NotFound(error),
                    ErrorNames.PlaylistPlaylistAccessForbidden => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HasPermission(Permission.CreatePlaylist)]
    [HttpPost("new")]
    public async Task<IActionResult> CreatePlaylist(PostPlaylistDto request, CancellationToken cancellationToken)
    {
        var result = await _playlistService.CreatePlaylistAsync(request, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Created,
            onFailure: error =>
            {
                return error.Code switch
                {
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HasPermission(Permission.DeletePlaylist)]
    [HttpDelete("delete/{playlistId}")]
    public async Task<IActionResult> DeletePlaylist(Guid playlistId, CancellationToken cancellationToken)
    {
        var result = await _playlistService.DeletePlaylistAsync(playlistId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Created,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.PlaylistPlaylistWasNotFound => NotFound(error),
                    ErrorNames.PlaylistDeleteAccessForbidden => StatusCode(StatusCodes.Status403Forbidden, error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HasPermission(Permission.EditPlaylist)]
    [HttpPost("{playlistId:guid}/add-question/{questionId:guid}")]
    public async Task<IActionResult> AddQuestionToPlaylist(Guid playlistId, Guid questionId,
        CancellationToken cancellationToken)
    {
        var result = await _playlistService.AddQuestionToPlaylistAsync(playlistId, questionId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Created,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.PlaylistPlaylistWasNotFound => NotFound(error),
                    ErrorNames.PlaylistAddQuestionAccessForbidden => StatusCode(StatusCodes.Status403Forbidden, error),
                    ErrorNames.PlaylistQuestionWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }

    [HasPermission(Permission.DeletePlaylist)]
    [HttpDelete("{playlistId:guid}/remove-question/{questionId:guid}")]
    public async Task<IActionResult> RemoveQuestionFromPlaylist(Guid playlistId, Guid questionId,
        CancellationToken cancellationToken)
    {
        var result = await _playlistService.RemoveQuestionFromPlaylistAsync(playlistId, questionId, cancellationToken);
        
        return result.Match<IActionResult>(
            onSuccess: Ok,
            onFailure: error =>
            {
                return error.Code switch
                {
                    ErrorNames.PlaylistPlaylistWasNotFound => NotFound(error),
                    ErrorNames.PlaylistDeleteQuestionAccessForbidden => StatusCode(StatusCodes.Status403Forbidden, error),
                    ErrorNames.PlaylistQuestionWasNotFound => NotFound(error),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error)
                };
            });
    }
}
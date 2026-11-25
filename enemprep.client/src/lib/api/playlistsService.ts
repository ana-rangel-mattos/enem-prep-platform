import { ApiClient } from "@/lib/api/apiClient";
import {
  IPlaylistDetailsResponse,
  IPlaylistResponse,
  IPostPlaylistRequest,
} from "@/lib/types/playlist.types";

class PlaylistsService extends ApiClient {
  constructor() {
    super("playlists");
  }

  GetUserPlaylists(
    filter: ISearchParams,
  ): Promise<IPagedResponse<IPlaylistResponse>> {
    return this.get("", filter);
  }

  GetPlaylistById(playlistId: string): Promise<IPlaylistDetailsResponse> {
    return this.get(`${playlistId}`);
  }

  CreatePlaylist(playlist: IPostPlaylistRequest): Promise<void> {
    return this.post("new", playlist);
  }

  DeletePlaylistById(playlistId: string): Promise<void> {
    return this.delete(`delete/${playlistId}`);
  }

  AddQuestionToPlaylist(playlistId: string, questionId: string): Promise<void> {
    return this.post(`${playlistId}/add-question/${questionId}`);
  }

  DeleteQuestionFromPlaylist(
    playlistId: string,
    questionId: string,
  ): Promise<void> {
    return this.delete(`${playlistId}/remove-question/${questionId}`);
  }
}

export const playlistsService = new PlaylistsService();

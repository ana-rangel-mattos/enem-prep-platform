import { IArticleResponse } from "@/lib/types/article.types";

export interface IPlaylistResponse {
  playlistId: string;
  userId: string;
  title: string;
  description: string;
  isPublic: boolean;
}

interface IPlaylistQuestionResponse {
  questionId: string;
  year: number;
  enunciation?: string;
  context?: string;
}

export interface IPlaylistDetailsResponse {
  playlistId: string;
  userId: string;
  title: string;
  description: string;
  isPublic: boolean;
  questions: IPlaylistQuestionResponse[];
  articles: IArticleResponse[];
}

export interface IPostPlaylistRequest {
  title: string;
  description: string;
  isPublic: boolean;
}

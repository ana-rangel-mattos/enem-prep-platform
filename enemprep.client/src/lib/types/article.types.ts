export interface IArticleBlock {
  id: string;
}

interface ITextBlock extends IArticleBlock {
  content: string;
}

interface IPlaylistBlock extends IArticleBlock {
  playlistId: string;
}

export interface IArticleResponse {
  articleId: string;
  userId: string;
  title: string;
  subtitle: string;
  updatedAt: string;
  createdAt: string;
}

export interface IArticleDetailsResponse {
  articleId: string;
  userId: string;
  title: string;
  subtitle: string;
  blocks: IArticleBlock[];
  updatedAt: string;
  createdAt: string;
}

export interface IPostArticleResponse {
  title: string;
  subtitle: string;
  blocks: IArticleBlock[];
}

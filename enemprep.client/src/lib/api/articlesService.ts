import { ApiClient } from "@/lib/api/apiClient";
import {
  IArticleDetailsResponse,
  IArticleResponse,
  IPostArticleResponse,
} from "@/lib/types/article.types";

class ArticlesService extends ApiClient {
  constructor() {
    super("articles");
  }

  GetArticles(
    params: ISearchParams,
  ): Promise<IPagedResponse<IArticleResponse>> {
    return this.get("", params);
  }

  GetArticleById(articleId: string): Promise<IArticleDetailsResponse> {
    return this.get(`${articleId}`);
  }

  CreateArticle(article: IPostArticleResponse): Promise<void> {
    return this.post("new", article);
  }

  UpdateArticle(
    articleId: string,
    article: IPostArticleResponse,
  ): Promise<void> {
    return this.put(`${articleId}`, article);
  }

  DeleteArticleById(articleId: string): Promise<void> {
    return this.delete(`${articleId}`);
  }
}

export const articlesService = new ArticlesService();

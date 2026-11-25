namespace EnemPrep.Domain.Result;

public static class ArticleErrors
{
    public static readonly Error InvalidPlaylistReference = new Error(
        ErrorNames.ArticleInvalidPlaylistReference,
        "Invalid exercise playlist reference.");
    
    public static Error ArticleWasNotFound(Guid articleId) => new Error(
        ErrorNames.ArticleArticleWasNotFound,
        $"Article ID {articleId} was not found.");

    public static readonly Error CannotDeleteForbiddenAccess = new Error(
        ErrorNames.ArticleCannotDeleteForbiddenAccess,
        $"Forbidden Access. You do not have permission to delete this article.");
}
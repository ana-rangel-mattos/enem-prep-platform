using System.Linq.Dynamic.Core;
using System.Reflection;
using EnemPrep.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EnemPrep.Domain.Common;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string sortBy) where T : class
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        var allowedProperties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var sortExpressions = new List<string>();

        foreach (var part in sortBy.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0 || !allowedProperties.Contains(tokens[0]))
                continue;

            var direction = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "descending"
                : "ascending";
            
            sortExpressions.Add($"{tokens[0]} {direction}");
        }

        return sortExpressions.Count > 0
            ? query.OrderBy(string.Join(", ", sortExpressions))
            : query;
    }

    public static IQueryable<Question> ApplySearch(this IQueryable<Question> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        return query.Where(q =>
            EF.Functions.ILike(q.Content, $"%{search}%"));
    }

    public static IQueryable<SavedQuestion> ApplySearch(this IQueryable<SavedQuestion> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        return query.Where(q => 
            EF.Functions.ILike(q.Question.Content, $"%{search}%"));
    }

    public static IQueryable<Question> ApplySubjectFilter(this IQueryable<Question> query, Guid? subjectId)
    {
        if (subjectId is null)
            return query;

        return query.Where(q => q.SubjectId == subjectId);
    }
}
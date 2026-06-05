namespace EnemPrep.Domain.Common;

public class ExamQueryFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string? SearchTerm { get; set; }
}
using System.Text.Json;

namespace EnemPrep.Domain.Common;

public static class SharedJsonOptions
{
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}
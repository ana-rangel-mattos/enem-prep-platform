using System.Text.Json;
using EnemPrep.Domain.Models;

namespace EnemPrep.Domain.Extensions;

public static class QuestionExtensions
{
    public static int ExtractYear(this Question question)
    {
        if (string.IsNullOrWhiteSpace(question.Content)) return 0;

        var root = ParseJsonElement(question.Content);
        
        int year = root.TryGetProperty("year", out JsonElement yearElement)
            ? yearElement.GetInt32()
            : 0;

        return year;
    }

    public static Dictionary<char, string> ExtractAlternatives(this Question question)
    {
        Dictionary<char, string> alternatives = new();
        
        if (string.IsNullOrWhiteSpace(question.Content)) return alternatives;

        var root = ParseJsonElement(question.Content);
        var hasValidAlternatives = root.TryGetProperty("alternatives", out JsonElement alternativesElement);
        if (hasValidAlternatives && alternativesElement.ValueKind == JsonValueKind.Array)
        {
            foreach (var alternative in alternativesElement.EnumerateArray())
            {
                var letter = (alternative.TryGetProperty("letter", out JsonElement letterElement)
                    ? letterElement.GetString()?[0]
                    : null);
                
                string? text = alternative.TryGetProperty("text", out JsonElement textElement)
                    ? textElement.GetString()
                    : string.Empty;

                if (letter is not null && text is not null)
                {
                    alternatives.Add(letter.Value, text);
                }
            }
        }

        return alternatives;
    }
    
    public static string? ExtractEnunciation(this Question question)
    {
        if (string.IsNullOrWhiteSpace(question.Content)) return string.Empty;

        var root = ParseJsonElement(question.Content);
        return root.TryGetProperty("alternativesIntroduction", out JsonElement enunciation)
            ? enunciation.GetString()
            : string.Empty;
    }

    public static JsonElement ExtractContent(this Question question)
    {
        return ParseJsonElement(question.Content);
    }
    
    public static string? ExtractContext(this Question question)
    {
        if (string.IsNullOrWhiteSpace(question.Content)) return string.Empty;

        var root = ParseJsonElement(question.Content);
        return root.TryGetProperty("context", out JsonElement context)
            ? context.GetString()
            : string.Empty;
    }

    public static char? ExtractCorrectAlternative(this Question question)
    {
        if (string.IsNullOrWhiteSpace(question.Content))
            return null;

        var root = ParseJsonElement(question.Content);
        return root.TryGetProperty("correctAlternative", out JsonElement correctAlternative)
            ? correctAlternative.GetString()?[0]
            : null;
    }

    public static string? ExtractAlternativeText(this Question question, string? alternativeLetter)
    {
        if (string.IsNullOrWhiteSpace(question.Content)) return string.Empty;
        
        var root = ParseJsonElement(question.Content);
        var hasValidAlternatives = root.TryGetProperty("alternatives", out JsonElement alternatives);
        
        if (hasValidAlternatives && alternatives.ValueKind == JsonValueKind.Array)
        {
            foreach (var alternative in alternatives.EnumerateArray())
            {
                var letter = alternative.TryGetProperty("letter", out JsonElement letterElement)
                    ? letterElement.GetString()
                    : string.Empty;

                if (!string.IsNullOrEmpty(letter) && letter.Equals(alternativeLetter))
                {
                    return alternative.TryGetProperty("text", out JsonElement textElement)
                        ? textElement.GetString()
                        : string.Empty;
                }
            }
        }

        return string.Empty;
    }
    
    private static JsonElement ParseJsonElement(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }
}
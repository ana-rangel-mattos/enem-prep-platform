using EnemPrep.Domain.Enums;

namespace EnemPrep.Services;

public static class ScoreCalculator
{
    private static (double Min, double Max) GetAreaBonus(SubjectName area) => area switch
    {
        SubjectName.Mathematics => (350.0, 1000.0),
        SubjectName.NaturalSciences => (350, 860.0),
        SubjectName.Humanities => (320.0, 840.0),
        SubjectName.Languages => (300.0, 770.0),
        _ => (200, 700.0)
    };

    public static float CalculateScore(int correctAnswers, SubjectName area)
    {
        if (correctAnswers <= 0) return (float)GetAreaBonus(area).Min;
        if (correctAnswers >= 45) return (float)GetAreaBonus(area).Max;

        var (min, max) = GetAreaBonus(area);

        double factor = correctAnswers / 45.0;
        double calculateScore = min + factor * (max - min);

        return (float)calculateScore;
    }
}
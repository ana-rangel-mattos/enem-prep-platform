namespace EnemPrep.Domain.Enums;

public enum Permission
{
    // Admin Permissions
    CreateQuestions = 1,
    ReadContent = 2,
    DeleteUser = 3,
    DeleteQuestion = 4,
    EditQuestion = 5,
    
    // Teacher Permissions
    CreateArticle = 6,
    DeleteArticle = 7,
    EditArticle = 8,
    CreateExerciseList = 9,
    DeleteExerciseList = 10,
    EditExerciseList = 11
}
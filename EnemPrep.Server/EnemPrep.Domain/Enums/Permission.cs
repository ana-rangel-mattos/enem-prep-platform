namespace EnemPrep.Domain.Enums;

public enum Permission
{
    // Student Permissions
    ReadContent = 2,
    
    // Admin Permissions
    CreateQuestions = 1,
    DeleteUser = 3,
    DeleteQuestion = 4,
    EditQuestion = 5,
    CreateSubject = 17,
    DeleteSubject = 18,
    
    // Teacher Permissions
    CreateArticle = 6,
    DeleteArticle = 7,
    EditArticle = 8,
    
    // Shared Permissions Student - Teacher
    ReadArticle = 12,
    ReadPlaylist = 13,
    CreatePlaylist = 14,
    DeletePlaylist = 15,
    EditPlaylist = 16,
}
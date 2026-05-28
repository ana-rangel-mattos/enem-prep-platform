namespace EnemPrep.Persistence.Constants;

public static class ErrorNames
{
    public const string LoginUserNotFound = "Login.UserNotFound";
    public const string LoginInvalidPassword = "Login.InvalidPassword";
    
    public const string LogoutFailedToLogout = "Logout.FailedToLogout";
    
    public const string RegisterUserAlreadyExists = "Register.UserAlreadyExists";
    public const string RegisterInvalidInvitationCode = "Register.InvalidInvitationCode";
    public const string RegisterFailedRegister = "Register.FailedRegister";

    public const string SubjectNotFound = "Subject.NotFound";
    public const string SubjectNullId = "Subject.NullId";

    public const string QuestionNotFound = "Question.NotFound";
    public const string QuestionNullId = "Question.NullId";
    public const string QuestionNullPublisherId = "Question.NullPublisherId";
    public const string QuestionPublisherIsNotLoggedUser = "Question.QuestionPublisherIsNotLoggedUser";

    public const string SolvedQuestionPrivateUser = "SolvedQuestion.PrivateUser";
    public const string SolvedQuestionNotFound = "SolvedQuestion.SolvedQuestionNotFound";
    public const string SolvedQuestionNullId = "SolvedQuestion.NullId";
    public const string SolvedQuestionNullUserId = "SolvedQuestion.NullUserId";
    public const string SolvedQuestionInexistentUser = "SolvedQuestion.UserDoesNotExist";
    public const string SolvedQuestionQuestionNotFound = "SolvedQuestion.QuestionNotFound";
    
    public const string UserUserNotFound = "User.UserNotFound";
    public const string UserNullUserId = "User.NullUserId";

    public const string UserGoalNullUserId = "UserGoal.NullUserId";
    public const string UserGoalUserNotFound = "UserGoal.UserNotFound";
    public const string UserGoalNotFound = "UserGoal.UserGoalNotFound";
    public const string UserGoalAlreadyExists = "UserGoal.UserGoalAlreadyExists";

    public const string SavedQuestionUserIsNotLoggedIn = "SavedQuestion.UserIsNotLoggedIn";
    public const string SavedQuestionQuestionNotFound = "SavedQuestion.QuestionNotFound";
    public const string SavedQuestionSavedQuestionNotFound = "SavedQuestion.SavedQuestionNotFound";
}
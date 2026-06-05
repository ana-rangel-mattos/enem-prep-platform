using EnemPrep.Domain.Enums;

namespace EnemPrep.Domain.Extensions;

public static class ExamStatusExtensions
{
    public static bool HasValidNewStatus(this ExamStatus currentStatus, ExamStatus newStatus)
    {
        if (currentStatus >= newStatus) 
            return false;

        if (currentStatus == ExamStatus.Finished && newStatus == ExamStatus.Canceled)
            return false;

        if (currentStatus == ExamStatus.Canceled)
            return false;
        
        if (newStatus - currentStatus == 1)
            return true;

        return newStatus == ExamStatus.Canceled;
    }
}


using FocusLifePlus.Domain.Common.Enums;

namespace FocusLifePlus.Shared.Helpers;

public static class TaskHelper
{
    public static bool IsTaskCompletable(FocusTaskStatus currentStatus)
    {
        return currentStatus != FocusTaskStatus.Done && 
               currentStatus != FocusTaskStatus.Cancelled;
    }

    public static bool IsTaskEditable(FocusTaskStatus currentStatus)
    {
        return currentStatus != FocusTaskStatus.Done && 
               currentStatus != FocusTaskStatus.Cancelled;
    }
} 
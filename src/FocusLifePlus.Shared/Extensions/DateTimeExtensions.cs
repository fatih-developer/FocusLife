namespace FocusLifePlus.Shared.Extensions;

public static class DateTimeExtensions
{
    public static bool IsInPast(this DateTime date)
    {
        return date < DateTime.Now;
    }

    public static bool IsInFuture(this DateTime date)
    {
        return date > DateTime.Now;
    }

    public static bool IsToday(this DateTime date)
    {
        return date.Date == DateTime.Today;
    }

    public static bool IsOverdue(this DateTime dueDate)
    {
        return dueDate < DateTime.Now;
    }
} 
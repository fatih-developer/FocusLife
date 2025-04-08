namespace FocusLifePlus.Shared.Constants;

public static class AppConstants
{
    public static class Cache
    {
        public const int DefaultExpirationMinutes = 60;
        public const string TaskListKey = "TaskList";
    }

    public static class Validation
    {
        public const int MaxTitleLength = 200;
        public const int MaxDescriptionLength = 1000;
    }

    public static class Pagination
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 50;
    }
} 
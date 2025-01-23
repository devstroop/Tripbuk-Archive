namespace Tripbuk.Server.Extensions;

public static class DateTimeExtensions
{
    // GetTimeAgo
    public static string GetTimeAgo(this DateTime input)
    {
        var timeSpan = DateTime.Now - input;

        if (timeSpan <= TimeSpan.FromSeconds(60))
        {
            return "Just now";
        }
        if (timeSpan <= TimeSpan.FromMinutes(1))
        {
            return $"{timeSpan.Seconds} seconds ago";
        }
        if (timeSpan <= TimeSpan.FromMinutes(2))
        {
            return "A minute ago";
        }
        if (timeSpan <= TimeSpan.FromHours(1))
        {
            return $"{timeSpan.Minutes} minutes ago";
        }
        if (timeSpan <= TimeSpan.FromHours(2))
        {
            return "An hour ago";
        }
        if (timeSpan <= TimeSpan.FromDays(1))
        {
            return $"{timeSpan.Hours} hours ago";
        }
        if (timeSpan <= TimeSpan.FromDays(2))
        {
            return "Yesterday";
        }
        if (timeSpan <= TimeSpan.FromDays(30))
        {
            return $"{timeSpan.Days} days ago";
        }
        if (timeSpan <= TimeSpan.FromDays(60))
        {
            return "A month ago";
        }
        if (timeSpan <= TimeSpan.FromDays(365))
        {
            return $"{timeSpan.Days / 30} months ago";
        }
        if (timeSpan <= TimeSpan.FromDays(730))
        {
            return "A year ago";
        }

        return $"{timeSpan.Days / 365} years ago";
    }
}
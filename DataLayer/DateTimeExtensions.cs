namespace DataLayer;
public static class DateTimeExtensions
{
    public static DateTime ClearNow()
    {
        DateTime now = DateTime.Now;
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Local);
    }
}

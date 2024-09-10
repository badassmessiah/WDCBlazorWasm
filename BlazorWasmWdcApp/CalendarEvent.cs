public class CalendarEvent
{
    public string Subject { get; set; }
    public DateTimeTimeZone Start { get; set; }
    public DateTimeTimeZone End { get; set; }

    //public CalendarEvent(string subject, DateTime startDateTime)
    //{
    //    Subject = subject;
    //    var dateAtStartOfTheDay = startDateTime.Date;

    //    Start = new DateTimeTimeZone
    //    {
    //        DateTime = dateAtStartOfTheDay.ToString("yyyy-MM-ddTHH:mm:ss"),
    //        TimeZone = "Asia/Tbilisi"
    //    };

    //    End = new DateTimeTimeZone
    //    {
    //        DateTime = dateAtStartOfTheDay.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
    //        TimeZone = "Asia/Tbilisi"
    //    };
    //}
}

public class DateTimeTimeZone
{
    public string DateTime { get; set; }
    public string TimeZone { get; set; } = "Asia/Tbilisi";
}

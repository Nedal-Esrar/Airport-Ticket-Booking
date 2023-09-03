namespace BusinessLogic.Services.DateTimeProvider;

public class DateTimeProvider : IDateTimeProvider
{
  public DateTime GetCurrentDateTime()
  {
    return DateTime.Now;
  }
}
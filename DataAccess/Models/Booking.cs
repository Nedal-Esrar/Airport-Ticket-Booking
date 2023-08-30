using DataAccess.Csv;

namespace DataAccess.Models;

public class Booking : ICsvWritable
{
  public int Id { get; set; }

  public Passenger Passenger { get; set; }
  
  public Flight Flight { get; set; }
  
  public FlightClass BookingClass { get; set; }
  
  public DateTime BookingDate { get; set; }

  public string GetCsvRecord() =>
    $"{Id},{Passenger.Id},{Flight.Id},{(int)BookingClass},{BookingDate}";
  
  public static string GetHeader() => ModelsCsvHeaders.BookingCsvHeader;
}
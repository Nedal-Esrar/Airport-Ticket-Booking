using DataAccessLayer.Csv;

namespace DataAccessLayer.Models;

public class Passenger : ICsvWritable
{
  public int Id { get; init; }
  
  public string Name { get; init; }

  public string GetCsvRecord() =>
    $"{Id},{Name}";
  
  public static string GetHeader() => ModelsCsvHeaders.PassengerCsvHeader;
}
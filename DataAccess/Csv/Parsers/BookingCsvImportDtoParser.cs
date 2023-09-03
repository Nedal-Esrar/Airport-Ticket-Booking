using System.Globalization;
using DataAccess.Csv.Dtos;
using DataAccess.Models;

namespace DataAccess.Csv.Parsers;

public class BookingCsvImportDtoParser : IParser<BookingCsvImportDto>
{
  public BookingCsvImportDto? Parse(string[] fields)
  {
    if (typeof(BookingCsvImportDto).GetProperties().Length != fields.Length)
    {
      return null;
    }

    return new BookingCsvImportDto
    {
      Id = int.TryParse(fields[0], out var id) ? id : null,
      PassengerId = int.TryParse(fields[1], out var passengerId) ? passengerId : null,
      FlightId = int.TryParse(fields[2], out var flightId) ? flightId : null,
      FlightClass = Enum.TryParse<FlightClass>(fields[3], out var flightClass) ? flightClass : null,
      BookingDate = DateTime.TryParse(fields[4], CultureInfo.InvariantCulture, out var date) ? date : null
    };
  }
}
using DataAccessLayer.Csv.Dtos;
using DataAccessLayer.Models;

namespace DataAccessLayer.Csv.Parsers;

public class FlightCsvImportDtoParser : IParser<FlightCsvImportDto>
{
  public FlightCsvImportDto? Parse(string[] fields)
  {
    if (typeof(FlightCsvImportDto).GetProperties().Length + Enum.GetValues<FlightClass>().Length - 1 != fields.Length)
    {
      return null;
    }

    var flightClasses = fields[6].Split(';');
    var flightPrices = fields[7].Split(';');
    var flightCapacities = fields[8].Split(';');

    if (!AreLengthsValid(flightClasses, flightPrices, flightCapacities))
    {
      return null;
    }

    var classesDetails = Enumerable
      .Range(0, flightClasses.Length)
      .Select(index => new FlightClassDetailsCsvImportDto
      {
        Class = Enum.TryParse<FlightClass>(flightClasses[index], out var flightClass) ? flightClass : null,
        Price = double.TryParse(flightPrices[index], out var price) ? price : null,
        Capacity = int.TryParse(flightCapacities[index], out var capacity) ? capacity : null
      })
      .ToList();

    return new FlightCsvImportDto
    {
      Id = int.TryParse(fields[0], out var id) ? id : null,
      DepartureCountry = fields[1].Trim(),
      DestinationCountry = fields[2].Trim(),
      DepartureDate = DateTime.TryParse(fields[3], out var date) ? date : null,
      DepartureAirport = fields[4].Trim(),
      ArrivalAirport = fields[5].Trim(),
      Classes = classesDetails
    };
  }

  private static bool AreLengthsValid(string[] flightClasses, string[] flightPrices, string[] flightCapacities)
  {
    if (!DoLengthsMatch(flightClasses, flightPrices, flightCapacities))
    {
      return false;
    }

    return flightClasses.Length <= Enum.GetValues<FlightClass>().Length;
  }

  private static bool DoLengthsMatch(string[] flightClasses, string[] flightPrices, string[] flightCapacities)
  {
    return flightClasses.Length == flightPrices.Length &&
           flightClasses.Length == flightCapacities.Length;
  }
}
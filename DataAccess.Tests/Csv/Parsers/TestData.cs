using DataAccess.Csv.Dtos;
using DataAccess.Models;

namespace DataAccess.Tests.Csv.Parsers;

public class TestData
{
  public static IEnumerable<object[]> PassengersToParse {
    get
    {
      yield return new object[]
      {
        new[] { "1", "GG" },
        new PassengerCsvImportDto
        {
          Id = 1,
          Name = "GG"
        }
      };

      yield return new object[]
      {
        new[] { "Let's do this", "BG" },
        new PassengerCsvImportDto
        {
          Id = null,
          Name = "BG"
        }
      };
    }
  }

  public static IEnumerable<object[]> BookingsToParse
  {
    get
    {
      yield return new object[]
      {
        new[] { "9", "4", "3", "1", "2020/12/30" },
        new BookingCsvImportDto
        {
          Id = 9,
          PassengerId = 4,
          FlightId = 3,
          FlightClass = FlightClass.Business,
          BookingDate = new DateTime(2020, 12, 30)
        }
      };
      
      yield return new object[]
      {
        new[] { "GG", "4", "yt", "0", "wrgwrg/12/30" },
        new BookingCsvImportDto
        {
          Id = null,
          PassengerId = 4,
          FlightId = null,
          FlightClass = FlightClass.Economy,
          BookingDate = null
        }
      };
      
      yield return new object[]
      {
        new[] { "9", "not a valid id", "3", "20g", "2020/12/30" },
        new BookingCsvImportDto
        {
          Id = 9,
          PassengerId = null,
          FlightId = 3,
          FlightClass = null,
          BookingDate = new DateTime(2020, 12, 30)
        }
      };
    }
  }

  public static IEnumerable<object[]> FlightsToParse
  {
    get
    {
      yield return new object[]
      {
        new[] { "1", "AA", "At", "d;gkn", "fa", "qwer", "1;0;2", "10.0;20.0;10.0", "1;1;1" },
        new FlightCsvImportDto
        {
          Id = 1,
          DepartureCountry = "AA",
          DestinationCountry = "At",
          DepartureDate = null,
          DepartureAirport = "fa",
          ArrivalAirport = "qwer",
          Classes = new List<FlightClassDetailsCsvImportDto>
          {
            new()
            {
              Class = FlightClass.Business,
              Price = 10.0,
              Capacity = 1
            },
            new()
            {
              Class = FlightClass.Economy,
              Price = 20.0,
              Capacity = 1
            },
            new()
            {
              Class = FlightClass.FirstClass,
              Price = 10.0,
              Capacity = 1
            }
          }
        }
      };
    }
  }
}
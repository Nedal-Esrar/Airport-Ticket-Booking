using DataAccess.Csv.Dtos;
using DataAccess.Models;

namespace DataAccess.Tests.Csv.Validators;

public class TestData
{
  private static readonly List<object[]> _validObjects = new()
  {
    new object[]
    {
      new PassengerCsvImportDto
      {
        Id = 1,
        Name = "GGGG"
      }
    },
    new object[]
    {
      new BookingCsvImportDto
      {
        Id = 9,
        PassengerId = 4,
        FlightId = 3,
        FlightClass = FlightClass.Business,
        BookingDate = new DateTime(2020, 12, 30)
      }
    }
  };

  private static readonly List<object?[]> _invalidObjects = new()
  {
    new object?[] { null },
    new object[]
    {
      new PassengerCsvImportDto
      {
        Id = null,
        Name = "BG"
      }
    },
    new object[]
    {
      new BookingCsvImportDto
      {
        Id = null,
        PassengerId = 4,
        FlightId = null,
        FlightClass = FlightClass.Economy,
        BookingDate = null
      }
    },
    new object[]
    {
      new BookingCsvImportDto
      {
        Id = 9,
        PassengerId = null,
        FlightId = 3,
        FlightClass = null,
        BookingDate = new DateTime(2020, 12, 30)
      }
    },
    new object[]
    {
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
            Price = -10.0,
            Capacity = 1
          }
        }
      }
    }
  };

  public static IEnumerable<object[]> ValidObjects => _validObjects;

  public static IEnumerable<object?[]> InvalidObjects => _invalidObjects;

  public static IEnumerable<object?[]> ObjectsWithErrorMessages
  {
    get
    {
      yield return new object[]
      {
        _validObjects[0], new List<string>()
      };
      
      yield return new object[]
      {
        _validObjects[1], new List<string>()
      };
      
      yield return new object?[]
      {
        _invalidObjects[0][0], new List<string>
        {
          "Invalid Object: # of parameters is not identical."
        }
      };
      
      yield return new object[]
      {
        _invalidObjects[1][0], new List<string>
        {
          "Id state is invalid.",
          "Name length should be at least 4 characters."
        }
      };
      
      yield return new object[]
      {
        _invalidObjects[2][0], new List<string>
        {
          "Id state is invalid.",
          "FlightId state is invalid.",
          "BookingDate state is invalid."
        }
      };
      
      yield return new object[]
      {
        _invalidObjects[3][0], new List<string>
        {
          "PassengerId state is invalid.",
          "FlightClass state is invalid."
        }
      };

      yield return new object[]
      {
        _invalidObjects[4][0], new List<string>
        {
          "DestinationCountry length should be at least 4 characters.",
          "DepartureDate state is invalid.",
          "DepartureAirport length should be at least 4 characters.",
          """
          For Flight Class #3
          Price should be greater than 0
          """
        }
      };
    }
  }
}
using DataAccess.Models;

namespace BusinessLogic.Tests.Services.TestData;

public class FlightTestData
{
  public static readonly List<Flight> Flights = new List<Flight>()
  {
    new()
    {
      Id = 1,
      Classes = new List<FlightClassDetails>
      {
        new() { Capacity = 4, Class = FlightClass.Economy }
      }
    },
    new()
    {
      Id = 2,
      Classes = new List<FlightClassDetails>
      {
        new() { Capacity = 5, Class = FlightClass.Business },
        new() { Capacity = 10, Class = FlightClass.FirstClass }
      }
    }
  };
  
  public static readonly List<Flight> ReadFromCsvFlights = new()
  {
    new() { Id = 1 },
    new() { Id = 1 },
    new() { Id = 2 }
  };
  
  
}
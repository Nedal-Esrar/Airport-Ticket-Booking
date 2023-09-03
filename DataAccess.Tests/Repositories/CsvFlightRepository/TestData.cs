using DataAccess.Models;
using DataAccess.SearchCriteria;

namespace DataAccess.Tests.Repositories;

public class TestData
{
  public readonly static List<Flight> Flights = new()
  {
    new()
    {
      Id = 1,
      ArrivalAirport = "GG",
      DepartureAirport = "G",
      DepartureCountry = "GGG",
      DestinationCountry = "GGGG",
      DepartureDate = new DateTime(2020, 12, 30),
      Classes = new List<FlightClassDetails>
      {
        new()
        {
          Capacity = 2,
          Class = FlightClass.Economy,
          Price = 20.0m
        }
      }
    },
    new()
    {
      Id = 2,
      ArrivalAirport = "GGGGG",
      DepartureAirport = "BG",
      DepartureCountry = "wt3",
      DestinationCountry = "341f",
      DepartureDate = new DateTime(2020, 12, 30),
      Classes = new List<FlightClassDetails>
      {
        new()
        {
          Capacity = 1,
          Class = FlightClass.Business,
          Price = 10.0m
        }
      }
    },
    new()
    {
      Id = 1,
      DestinationCountry = "asd"
    }
  };

  public static IEnumerable<object[]> CriteriaTestData
  {
    get
    {
      yield return new object[]
      {
        new FlightSearchCriteria
        {
          DepartureAirport = "BG",
          Price = 10.0m,
          Class = FlightClass.Business
        },
        new List<Flight>
        {
          Flights[1]
        }
      };

      yield return new object[]
      {
        new FlightSearchCriteria
        {
          ArrivalAirport = "GG",
          DepartureDate = new DateTime(2020, 12, 30)
        },
        new List<Flight>
        {
          Flights[0]
        }
      };

      yield return new object[]
      {
        new FlightSearchCriteria
        {
          DepartureDate = new DateTime(2020, 12, 30)
        },
        new List<Flight>
        {
          Flights[0], Flights[1]
        }
      };
    }
  }
}
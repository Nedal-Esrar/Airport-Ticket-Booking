using DataAccess.Models;
using DataAccess.SearchCriteria;

namespace DataAccess.Tests.Repositories.BookingRepository;

public class TestData
{
  public readonly static List<Booking> Bookings = new()
  {
    new()
    {
      Id = 1,
      BookingDate = new DateTime(2020, 12, 30),
      Flight = new()
      {
        Id = 1,
        ArrivalAirport = "GG",
        DepartureAirport = "GB",
        DepartureCountry = "134",
        DestinationCountry = "452df",
        DepartureDate = new DateTime(2021, 11, 30),
        Classes = new List<FlightClassDetails>()
        {
          new()
          {
            Class = FlightClass.FirstClass
          },
          new()
          {
            Class = FlightClass.Business
          }
        },
      },
      Passenger = new() { Id = 1 },
      BookingClass = FlightClass.FirstClass
    },
    new()
    {
      Id = 2,
      BookingDate = new DateTime(2019, 12, 30),
      Flight = new()
      {
        Id = 2,
        ArrivalAirport = "G31",
        DepartureAirport = "GB",
        DepartureCountry = "134134",
        DestinationCountry = "452df",
        DepartureDate = new DateTime(2021, 11, 30),
        Classes = new List<FlightClassDetails>()
        {
          new()
          {
            Class = FlightClass.FirstClass
          },
          new()
          {
            Class = FlightClass.Business
          }
        },
      },
      Passenger = new() { Id = 2 },
      BookingClass = FlightClass.Business
    },
    new()
    {
      Id = 3,
      BookingDate = new DateTime(2020, 12, 25),
      Flight = new()
      {
        Id = 3,
        ArrivalAirport = "G3xv1",
        DepartureAirport = "GdfB",
        DepartureCountry = "1341asdf34",
        DestinationCountry = "452ddff",
        DepartureDate = new DateTime(2021, 11, 30),
        Classes = new List<FlightClassDetails>()
        {
          new()
          {
            Class = FlightClass.Economy
          },
          new()
          {
            Class = FlightClass.Business
          }
        },
      },
      Passenger = new() { Id = 1 },
      BookingClass = FlightClass.Business
    },
    new()
    {
      Id = 1
    }
  };

  public static IEnumerable<object[]> GetPassengerBookingsTestData
  {
    get
    {
      yield return new object[] { 1, new List<Booking> { Bookings[0], Bookings[2] } };

      yield return new object[] { 2, new List<Booking> { Bookings[1] } };

      yield return new object[] { 1000, new List<Booking>() };
    }
  }

  public static IEnumerable<object[]> GetBookingsForFlightWithClassTestData
  {
    get
    {
      yield return new object[]
      {
        3, FlightClass.Business, new List<Booking> { Bookings[2] }
      };

      yield return new object[]
      {
        3, FlightClass.Economy, new List<Booking>()
      };

      yield return new object[]
      {
        2, FlightClass.Business, new List<Booking> { Bookings[1] }
      };
    }
  }

  public static IEnumerable<object[]> GetMatchingCriteriaTestData
  {
    get
    {
      yield return new object[]
      {
        new BookingSearchCriteria
        {
          ArrivalAirport = "GG",
          DepartureDate = new DateTime(2021, 11, 30),
          Class = FlightClass.FirstClass
        },
        new List<Booking> { Bookings[0] }
      };

      yield return new object[]
      {
        new BookingSearchCriteria
        {
          DestinationCountry = "452df",
        },
        new List<Booking> { Bookings[0], Bookings[1] }
      };
    }
  }
}
using DataAccess.Csv.Dtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;
using FluentAssertions.Execution;

namespace DataAccess.Tests.Csv.Mappers;

public class FlightCsvImportDtoToFlightMapperTests
{
  [Fact]
  public void Map_ValidatedFlightCsvImportDto_ShouldReturnTheExpectedFlight()
  {
    var dto = new FlightCsvImportDto
    {
      Id = 1,
      DepartureCountry = "Palestine",
      DestinationCountry = "Japan",
      DepartureDate = new DateTime(2020, 12, 30),
      DepartureAirport = "GGGGGG",
      ArrivalAirport = "BBBBBBB",
      Classes = new List<FlightClassDetailsCsvImportDto>
      {
        new()
        {
          Capacity = 1,
          Class = FlightClass.Business,
          Price = 10.0
        }
      }
    };

    var expected = new Flight
    {
      Id = 1,
      DepartureCountry = "Palestine",
      DestinationCountry = "Japan",
      DepartureDate = new DateTime(2020, 12, 30),
      DepartureAirport = "GGGGGG",
      ArrivalAirport = "BBBBBBB",
      Classes = new List<FlightClassDetails>
      {
        new()
        {
          Capacity = 1,
          Class = FlightClass.Business,
          Price = 10.0m
        }
      }
    };

    var sut = new FlightCsvImportDtoToFlightMapper();

    var returnedFlight = sut.Map(dto);

    returnedFlight.Should()
      .NotBeNull()
      .And
      .BeEquivalentTo(expected);
  }
}
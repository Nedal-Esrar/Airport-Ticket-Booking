using DataAccess.Csv.Dtos;
using DataAccess.Models;

namespace DataAccess.Csv.Mappers;

public class FlightCsvImportDtoToFlightMapper : IMapper<FlightCsvImportDto, Flight>
{
  public Flight? Map(FlightCsvImportDto? dto)
  {
    if (dto is null)
    {
      return null;
    }
    
    var flightClassDetails = dto.Classes
      .Select(classDetailsDto => new FlightClassDetails
      {
        Class = (FlightClass) classDetailsDto.Class!,
        Price = (decimal) classDetailsDto.Price!,
        Capacity = (int) classDetailsDto.Capacity!
      })
      .ToList();

    return new Flight
    {
      Id = (int) dto.Id!,
      DepartureCountry = dto.DepartureCountry!,
      DestinationCountry = dto.DestinationCountry!,
      DepartureDate = (DateTime)dto.DepartureDate!,
      DepartureAirport = dto.DepartureAirport!,
      ArrivalAirport = dto.ArrivalAirport!,
      Classes = flightClassDetails
    };
  }
}
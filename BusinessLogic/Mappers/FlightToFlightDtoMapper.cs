using BusinessLogic.PresentationLayerDtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Mappers;

public class FlightToFlightDtoMapper : IMapper<Flight, FlightDto>
{
  private readonly IMapper<FlightClassDetails, FlightClassDetailsDto> _classDetailsMapper;

  public FlightToFlightDtoMapper(IMapper<FlightClassDetails, FlightClassDetailsDto> classDetailsMapper)
  {
    _classDetailsMapper = classDetailsMapper;
  }

  public FlightDto? Map(Flight? input)
  {
    if (input is null)
    {
      return null;
    }

    return new FlightDto
    {
      Id = input.Id,
      DepartureCountry = input.DepartureCountry,
      DestinationCountry = input.DestinationCountry,
      DepartureDate = input.DepartureDate,
      DepartureAirport = input.DepartureAirport,
      ArrivalAirport = input.ArrivalAirport,
      Classes = input
        .Classes
        .Select(details => _classDetailsMapper.Map(details))
        .ToList()
    };
  }
}
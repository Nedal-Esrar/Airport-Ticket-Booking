using BusinessLogic.PresentationLayerDtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Mappers;

public class FlightClassDetailsToFlightClassDetailsDtoMapper : IMapper<FlightClassDetails, FlightClassDetailsDto>
{
  public FlightClassDetailsDto? Map(FlightClassDetails? input)
  {
    if (input is null)
    {
      return null;
    }

    return new FlightClassDetailsDto
    {
      Class = input.Class,
      Capacity = input.Capacity,
      Price = input.Price
    };
  }
}
using BusinessLogic.PresentationLayerDtos;
using DataAccess.Csv.Mappers;
using DataAccess.Models;

namespace BusinessLogic.Mappers;

public class PassengerToPassengerDtoMapper : IMapper<Passenger, PassengerDto>
{
  public PassengerDto? Map(Passenger? input)
  {
    if (input is null)
    {
      return null;
    }

    return new PassengerDto
    {
      Id = input.Id,
      Name = input.Name
    };
  }
}
using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccessLayer.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations;

public class PassengerService : IPassengerService
{
  private readonly IPassengerRepository _passengerRepository;

  public PassengerService(IPassengerRepository passengerRepository)
  {
    _passengerRepository = passengerRepository;
  }

  public PassengerDto? GetById(int id)
  {
    var passenger = _passengerRepository.GetById(id);

    return passenger == null ? null : new PassengerDto(passenger);
  }
}
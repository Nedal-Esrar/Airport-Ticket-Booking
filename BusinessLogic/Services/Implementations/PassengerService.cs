using BusinessLogic.PresentationLayerDtos;
using BusinessLogic.Services.Interfaces;
using DataAccess.Csv.Mappers;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.Services.Implementations;

public class PassengerService : IPassengerService
{
  private readonly IPassengerRepository _passengerRepository;

  private readonly IMapper<Passenger, PassengerDto> _passengerMapper;

  public PassengerService(IPassengerRepository passengerRepository, IMapper<Passenger, PassengerDto> passengerMapper)
  {
    _passengerRepository = passengerRepository;

    _passengerMapper = passengerMapper;
  }

  public async Task<PassengerDto?> GetById(int id)
  {
    var passenger = await _passengerRepository.GetById(id);

    return _passengerMapper.Map(passenger);
  }
}
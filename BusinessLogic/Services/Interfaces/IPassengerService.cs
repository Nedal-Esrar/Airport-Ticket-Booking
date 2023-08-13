using BusinessLogic.PresentationLayerDtos;

namespace BusinessLogic.Services.Interfaces;

public interface IPassengerService
{
  PassengerDto? GetById(int id);
}
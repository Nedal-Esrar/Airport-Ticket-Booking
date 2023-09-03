using BusinessLogic.Services.Interfaces;
using PresentationLayer.CommandInvokers;
using PresentationLayer.DisplayUtilities;
using PresentationLayer.InputUtilities;

namespace PresentationLayer.UserInterfaces.Factories;

public class PassengerInterfaceFactory : IUserInterfaceFactory
{
  private readonly IBookingService _bookingService;
  
  private readonly IFlightService _flightService;

  private readonly IPassengerService _passengerService;

  public PassengerInterfaceFactory(
    IBookingService bookingService,
    IFlightService flightService,
    IPassengerService passengerService)
  {
    _bookingService = bookingService;

    _flightService = flightService;

    _passengerService = passengerService;
  }
  
  public async Task<IUserInterface?> Create()
  {
    var passengerId = InputParser.GetInput<int>(InputPrompts.PassengerIdPromptWithoutSkip,
      ParseFunctionsWithoutSkip.TryParseId);

    var passenger = await _passengerService.GetById(passengerId);

    if (passenger == null)
    {
      Console.WriteLine(PassengerMessages.PassengerNotFound);
      
      return null;
    }

    var invoker = new PassengerCommandInvoker(_flightService, _bookingService, passenger);

    return new PassengerInterface(invoker);
  }
}
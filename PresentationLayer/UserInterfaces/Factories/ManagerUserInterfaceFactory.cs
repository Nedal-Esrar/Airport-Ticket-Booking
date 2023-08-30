using BusinessLogic.Services.Interfaces;
using PresentationLayer.CommandInvokers;

namespace PresentationLayer.UserInterfaces.Factories;

public class ManagerInterfaceFactory : IUserInterfaceFactory
{
  private readonly IBookingService _bookingService;
  
  private readonly IFlightService _flightService;

  public ManagerInterfaceFactory(IBookingService bookingService, IFlightService flightService)
  {
    _bookingService = bookingService;

    _flightService = flightService;
  }
  
  public IUserInterface? Create()
  {
    var invoker = new ManagerCommandInvoker(_bookingService, _flightService);

    return new ManagerInterface(invoker);
  }
}